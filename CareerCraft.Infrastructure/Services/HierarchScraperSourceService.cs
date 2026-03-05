using CareerCraft.Core.Entities;
using CareerCraft.Core.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CareerCraft.Infrastructure.Services;

public class HierarchScraperSourceService : IVacancySourceService
{
    private readonly HttpClient _httpClient;
    public string SourceName => "HierarchScraper";

    public HierarchScraperSourceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExternalVacancy>> GetVacanciesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<ExternalVacancy>>("api/vacancies");
        return response ?? new List<ExternalVacancy>();
    }

    public async Task<ExternalVacancy?> GetVacancyByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ExternalVacancy>($"api/vacancies/{id}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
