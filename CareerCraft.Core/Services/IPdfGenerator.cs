namespace CareerCraft.Core.Services;

public interface IPdfGenerator : IAsyncDisposable
{
    Task GenerateFromHtmlAsync(string htmlPath, string pdfOutputPath);
}