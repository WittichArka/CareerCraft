using System.Text.Json.Serialization;

namespace CareerCraft.Core;

public class AiContext
{
    public string Model { get; set; } = "gemini-2.5-flash"; // Default based on confirmed availability
    public float Temperature { get; set; } = 0.5f;
    public string PromptName { get; set; } = string.Empty; // The name of the prompt template (YAML/MD)
    public Dictionary<string, string> AdditionalParameters { get; set; } = new();
}

public class AiPromptResult
{
    public bool Success { get; set; }
    public string RawResponse { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public int TokenUsage { get; set; }
}
