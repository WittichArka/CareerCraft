namespace CareerCraft.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Écrit le contenu d'une chaîne dans un fichier de manière asynchrone.
    /// </summary>
    public static async Task WriteToFileAsync(this string content, string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        // Création du répertoire si inexistant
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(filePath, content ?? string.Empty);
    }
}
