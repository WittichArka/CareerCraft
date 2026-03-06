using System.Threading.Tasks;

namespace CareerCraft.Core.Services;

public interface IAiService
{
    /// <summary>
    /// Executes an AI prompt with the provided context and input data.
    /// </summary>
    /// <typeparam name="T">The expected type of the response after deserialization.</typeparam>
    /// <param name="context">Context for the AI (model, temperature, prompt name).</param>
    /// <param name="inputData">The structured data to send with the prompt (will be serialized to JSON).</param>
    /// <returns>A deserialized object of type T.</returns>
    Task<T> GetAiResponseAsync<T>(AiContext context, object inputData) where T : class;

    /// <summary>
    /// Executes an AI prompt and returns the raw result.
    /// </summary>
    /// <param name="context">Context for the AI.</param>
    /// <param name="inputData">Input data.</param>
    /// <returns>AiPromptResult containing raw response and metadata.</returns>
    Task<AiPromptResult> ExecutePromptAsync(AiContext context, object inputData);
}
