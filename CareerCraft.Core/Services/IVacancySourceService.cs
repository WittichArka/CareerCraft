using CareerCraft.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerCraft.Core.Services;

public interface IVacancySourceService
{
    string SourceName { get; }
    Task<IEnumerable<ExternalVacancy>> GetVacanciesAsync();
    Task<ExternalVacancy?> GetVacancyByIdAsync(int id);
}
