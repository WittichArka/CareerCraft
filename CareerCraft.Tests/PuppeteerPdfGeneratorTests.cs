using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Services;
using CareerCraft.Core.Extensions;

using Microsoft.Extensions.Configuration;

namespace CareerCraft.Tests;
public class PdfGeneratorTests
{
    private readonly IPdfGenerator _generator;
    private readonly ITemplateService? _templateService = null;
    
    public PdfGeneratorTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Puppeteer:ExecutablePath"] = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            })
            .Build();

        // Dans un vrai test, on utiliserait un Mock ou une injection réelle
        _templateService = new RazorTemplateService();
        _generator = new PuppeteerPdfGenerator(configuration); 
    }

    [Fact]
    public async Task GenerateFromHtmlAsync_ShouldCreateFile()
    {
        // Arrange
        var viewModel = RazorTemplateServiceTests.GetSampleTemplateViewModel();
        var isDeleteTempFileAfterTest = false;
        await TestHtmlAndPdfGeneration(viewModel, "normal", isDeleteTempFileAfterTest);
        await TestHtmlAndPdfGeneration(viewModel, "large", isDeleteTempFileAfterTest);
    }

    private async Task TestHtmlAndPdfGeneration(Core.ViewModels.TemplateViewModel viewModel, string templateKey, bool isDeleteTempFileAfterTest)
    {
        string htmlContent = await _templateService.RenderAsync(templateKey, viewModel);

        var templatesFolder = Path.Combine(AppContext.BaseDirectory, "Templates");
        if (!Directory.Exists(templatesFolder))
        {
            Directory.CreateDirectory(templatesFolder);
        }
        var dtnow = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"Temp_{templateKey}_{dtnow}";
        var htmlFileName = $"{fileName}.html";
        var htmlPath = Path.Combine(templatesFolder, htmlFileName);
        var pdfFileName = $"{fileName}.pdf";
        var pdfOutputPath = Path.Combine(templatesFolder, pdfFileName);
        try
        {
            await htmlContent.WriteToFileAsync(htmlPath);
            // Act
            await _generator.GenerateFromHtmlAsync(htmlPath, pdfOutputPath);

            // Assert
            Assert.True(File.Exists(htmlPath));
            var info = new FileInfo(htmlPath);
            Assert.True(info.Length > 0);
            Assert.True(File.Exists(pdfOutputPath));
            info = new FileInfo(pdfOutputPath);
            Assert.True(info.Length > 0);
        }
        finally
        {
            if (isDeleteTempFileAfterTest)
            {
                if (File.Exists(htmlPath)) File.Delete(htmlPath);
                if (File.Exists(pdfOutputPath)) File.Delete(pdfOutputPath);
            }
        }
    }
}
