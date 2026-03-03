using CareerCraft.Core.Services;
using CareerCraft.Core.ViewModels;
using RazorLight;
using Microsoft.CodeAnalysis; // Nécessaire pour MetadataReference
using System.Reflection;

namespace CareerCraft.Infrastructure.Services;

public class RazorTemplateService : ITemplateService
{
    private readonly IRazorLightEngine _engine;

    public RazorTemplateService()
    {
        var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates");

        // On récupère toutes les références d'assemblage chargées
        var metadataReferences = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Cast<MetadataReference>()
            .ToArray(); // On transforme en tableau pour le builder

        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(templatePath)
            .UseMemoryCachingProvider()
            // On passe le tableau de références ici
            .AddMetadataReferences(metadataReferences) 
            .Build();
    }

    public async Task<string> RenderAsync<T>(string templateKey, T model)
    {
        var key = templateKey.Replace(".cshtml", "");
        return await _engine.CompileRenderAsync(key, model);
    }
}