using AutoMapper;
using CareerCraft.Core.Entities;
using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerCraft.Infrastructure.Services;

public class VacancySyncService : IVacancySyncService
{
    private readonly AppDbContext _context;
    private readonly IVacancySourceService _sourceService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public VacancySyncService(
        AppDbContext context, 
        IVacancySourceService sourceService, 
        IMapper mapper,
        IConfiguration configuration)
    {
        _context = context;
        _sourceService = sourceService;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<SyncAllResult> SyncAllAsync()
    {
        var result = new SyncAllResult();
        var externalVacancies = await _sourceService.GetVacanciesAsync();
        var syncDate = DateTime.UtcNow;

        var externalIds = externalVacancies.Select(v => v.Id.ToString()).ToList();
        
        // 1. Marquer comme inactifs ou supprimer les jobs locaux qui ne sont plus dans la source
        var existingLocalVacancies = await _context.Vacancies
            .Where(v => v.SourceName == _sourceService.SourceName)
            .ToListAsync();

        var idsToRemove = existingLocalVacancies
            .Where(v => !externalIds.Contains(v.ExternalId))
            .ToList();

        bool hardDelete = _configuration.GetValue<bool>("SyncSettings:HardDelete", false);

        foreach (var toRemove in idsToRemove)
        {
            if (hardDelete)
            {
                _context.Vacancies.Remove(toRemove);
                result.Deleted++;
            }
            else
            {
                toRemove.IsActive = false;
                toRemove.LastSyncDateTime = syncDate;
                result.Deactivated++;
            }
        }

        // 2. Ajouter ou mettre à jour les jobs provenant de la source
        foreach (var external in externalVacancies)
        {
            result.TotalProcessed++;
            var existing = existingLocalVacancies.FirstOrDefault(v => v.ExternalId == external.Id.ToString());

            if (existing == null)
            {
                var newVacancy = _mapper.Map<Vacancy>(external);
                newVacancy.SourceName = _sourceService.SourceName;
                newVacancy.ExternalId = external.Id.ToString();
                newVacancy.LastSyncDateTime = syncDate;
                _context.Vacancies.Add(newVacancy);
                result.Added++;
            }
            else
            {
                _mapper.Map(external, existing);
                existing.LastSyncDateTime = syncDate;
                existing.IsActive = true; // Réactiver si le job réapparaît
                result.Updated++;
            }
        }

        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<SyncByIdResult> SyncByIdAsync(int externalId)
    {
        var external = await _sourceService.GetVacancyByIdAsync(externalId);
        if (external == null)
        {
            return new SyncByIdResult { Success = false, Message = "Offre non trouvée à la source", Status = "NotFound" };
        }

        var syncDate = DateTime.UtcNow;
        var existing = await _context.Vacancies
            .FirstOrDefaultAsync(v => v.SourceName == _sourceService.SourceName && v.ExternalId == externalId.ToString());

        if (existing == null)
        {
            var newVacancy = _mapper.Map<Vacancy>(external);
            newVacancy.SourceName = _sourceService.SourceName;
            newVacancy.ExternalId = externalId.ToString();
            newVacancy.LastSyncDateTime = syncDate;
            _context.Vacancies.Add(newVacancy);
            await _context.SaveChangesAsync();
            return new SyncByIdResult { Success = true, Message = "Offre ajoutée", Status = "Added" };
        }
        else
        {
            _mapper.Map(external, existing);
            existing.LastSyncDateTime = syncDate;
            existing.IsActive = true;
            await _context.SaveChangesAsync();
            return new SyncByIdResult { Success = true, Message = "Offre mise à jour", Status = "Updated" };
        }
    }
}
