using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CareerCraft.Core;
using CareerCraft.Core.Services;
using Polly;
using Polly.Retry;

namespace CareerCraft.Infrastructure.Services;

public class SemanticKernelAiService : IAiService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SemanticKernelAiService> _logger;
    private readonly Kernel _kernel;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly JsonSerializerOptions _jsonOptions;

    public SemanticKernelAiService(IConfiguration configuration, ILogger<SemanticKernelAiService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        // Custom HttpClient with longer timeout (3 minutes) for slow machines
        var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(3) };

        var builder = Kernel.CreateBuilder();

        // Configure AI Providers (Gemini and OpenAI as examples)
        ConfigureAiProviders(builder, httpClient);

        _kernel = builder.Build();

        // Polly Retry Policy - Patient retry for low RPM (5/min) and slow hardware
        _retryPolicy = Policy
            .Handle<Exception>(ex => ex.Message.Contains("429") || ex.Message.Contains("500") || ex.Message.Contains("503"))
            .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(15),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"AI request failed. Retry {retryCount} after {timeSpan.TotalSeconds}s. Error: {exception.Message}");
                });

        // Tolerant JSON Options
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
            WriteIndented = true
        };
    }

    private void ConfigureAiProviders(IKernelBuilder builder, HttpClient httpClient)
    {
        var geminiKey = _configuration["Ai:Gemini:ApiKey"];
        if (!string.IsNullOrEmpty(geminiKey) && !geminiKey.Contains("YOUR_GEMINI"))
        {
            var geminiModelId = _configuration["Ai:Gemini:Model"] ?? "gemini-2.5-flash";
            builder.AddGoogleAIGeminiChatCompletion(
                modelId: geminiModelId,
                apiKey: geminiKey,
                httpClient: httpClient);
        }

        var openAiKey = _configuration["Ai:OpenAI:ApiKey"];
        if (!string.IsNullOrEmpty(openAiKey) && !openAiKey.Contains("YOUR_OPENAI"))
        {
            builder.AddOpenAIChatCompletion(
                modelId: _configuration["Ai:OpenAI:Model"] ?? "gpt-4",
                apiKey: openAiKey,
                httpClient: httpClient);
        }
    }

    public async Task<T> GetAiResponseAsync<T>(AiContext context, object inputData) where T : class
    {
        var result = await ExecutePromptAsync(context, inputData);
        if (!result.Success)
        {
            throw new Exception($"AI Error: {result.ErrorMessage}");
        }

        try
        {
            // Clean the response if it contains markdown code blocks
            string cleanedResponse = CleanMarkdown(result.RawResponse);
            var deserialized = JsonSerializer.Deserialize<T>(cleanedResponse, _jsonOptions);
            return deserialized ?? throw new Exception("Deserialization returned null.");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize AI response: {RawResponse}", result.RawResponse);
            throw;
        }
    }

    public async Task<AiPromptResult> ExecutePromptAsync(AiContext context, object inputData)
    {
        try
        {
            string promptFilePath = Path.Combine(AppContext.BaseDirectory, "AiPrompts", $"{context.PromptName}.yaml");
            if (!File.Exists(promptFilePath))
            {
                // Try .md if .yaml doesn't exist
                promptFilePath = Path.Combine(AppContext.BaseDirectory, "AiPrompts", $"{context.PromptName}.md");
            }

            if (!File.Exists(promptFilePath))
            {
                throw new FileNotFoundException($"Prompt template not found: {context.PromptName}");
            }

            string template = await File.ReadAllTextAsync(promptFilePath);
            
            // In a real implementation, you would use Semantic Kernel's template engine
            // For now, let's assume we use the kernel to invoke a function from this template
            var kernelFunction = _kernel.CreateFunctionFromPrompt(template, new PromptExecutionSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "temperature", context.Temperature }
                }
            });

            var jsonInput = JsonSerializer.Serialize(inputData, _jsonOptions);
            
            var kernelArguments = new KernelArguments();
            kernelArguments["input"] = jsonInput;
            foreach (var param in context.AdditionalParameters)
            {
                kernelArguments[param.Key] = param.Value;
            }

            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _kernel.InvokeAsync(kernelFunction, kernelArguments);
                return response.ToString();
            });

            return new AiPromptResult
            {
                Success = true,
                RawResponse = result
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI Execution failed for prompt {PromptName}", context.PromptName);
            return new AiPromptResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    private string CleanMarkdown(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        
        // Remove ```json and ``` markers
        if (input.StartsWith("```"))
        {
            int firstLineEnd = input.IndexOf('\n');
            if (firstLineEnd != -1)
            {
                input = input.Substring(firstLineEnd).Trim();
            }
            if (input.EndsWith("```"))
            {
                input = input.Substring(0, input.Length - 3).Trim();
            }
        }
        return input;
    }
}
