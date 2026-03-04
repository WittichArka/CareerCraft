namespace CareerCraft.Core.Services;

public interface ITemplateService
{
    /// <summary>
    /// Génère une chaîne HTML à partir d'un template Razor et d'un modèle de données.
    /// </summary>
    /// <param name="templateKey">Le nom du fichier (ex: "JobOfferTemplate")</param>
    /// <param name="model">L'objet contenant les données de l'IA et de l'offre</param>
    Task<string> RenderAsync<T>(string templateKey, T model);
}
