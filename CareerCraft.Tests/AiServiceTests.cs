using CareerCraft.Core;
using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CareerCraft.Tests;

public class AiServiceTests
{
    private readonly IAiService _aiService;

    public AiServiceTests()
    {
        // On charge la config (incluant les User Secrets pour la clé d'API)
        // Note: Les secrets doivent être définis pour le projet CareerCraft.Tests
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<AiServiceTests>() 
            .Build();

        var logger = new Mock<ILogger<SemanticKernelAiService>>().Object;
        _aiService = new SemanticKernelAiService(configuration, logger);
    }

    [Fact]
    public async Task MatchSkills_IntegrationTest()
    {
        // Arrange
        var context = new AiContext 
        { 
            PromptName = "MatchSkills",
            Temperature = 0.3f 
        };

        var inputData = new 
        {
            UserSkills = new[] { "C#", "ASP.NET Core", "SQL", "Docker" },
            JobDescription = "We are looking for a .NET Developer proficient in C#, ASP.NET Core and containerization with Docker."
        };

        // Act
        // On utilise dynamic pour tester la structure flexible du JSON
        var result = await _aiService.GetAiResponseAsync<MatchSkillsResult>(context, inputData);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Score > 0, "Le score devrait être supérieur à 0");
        Assert.NotEmpty(result.MatchingSkills);
        Assert.Contains("C#", result.MatchingSkills);
        Assert.NotNull(result.Analysis);
    }

    // Classe de test pour la désérialisation
    public class MatchSkillsResult
    {
        public int Score { get; set; }
        public List<string> MatchingSkills { get; set; } = new();
        public List<string> MissingSkills { get; set; } = new();
        public string Analysis { get; set; } = string.Empty;
    }
}
