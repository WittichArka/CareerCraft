using System.Text;
using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace CareerCraft.Core.Services;

public class PuppeteerPdfGenerator : IPdfGenerator
{
    private IBrowser _browser;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly string _executablePath;

    public PuppeteerPdfGenerator(IConfiguration configuration)
    {
        _executablePath = configuration["Puppeteer:ExecutablePath"] ?? @"C:\Program Files\Google\Chrome\Application\chrome.exe";
    }

    private async Task EnsureBrowserAsync()
    {
        if (_browser == null || _browser.IsClosed)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_browser == null || _browser.IsClosed)
                {
                    _browser = await Puppeteer.LaunchAsync(new LaunchOptions
                    {
                        Headless = true,
                        ExecutablePath = _executablePath,
                        Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
                    });
                }
            }
            finally { _semaphore.Release(); }
        }
    }

    public async Task GenerateFromHtmlAsync(string htmlPath, string pdfOutputPath)
    {
        await EnsureBrowserAsync();
        using var page = await _browser.NewPageAsync();
        
        await page.GoToAsync($"file:///{htmlPath.Replace("\\", "/")}", WaitUntilNavigation.Networkidle0);
        await page.EmulateMediaTypeAsync(MediaType.Screen);
        
        await page.PdfAsync(pdfOutputPath, new PdfOptions
        {
            PrintBackground = true,
            PreferCSSPageSize = true,
            MarginOptions = new MarginOptions { Top = "0px", Bottom = "0px", Left = "0px", Right = "0px" }
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser != null) await _browser.CloseAsync();
        _semaphore.Dispose();
    }
}