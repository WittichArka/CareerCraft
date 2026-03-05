using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CareerCraft.Core.Entities;

public class Vacancy
{
    public int Id { get; set; }
    
    // Identification de la source pour la synchro
    public string SourceName { get; set; } = string.Empty; // ex: HierarchScraper
    public string ExternalId { get; set; } = string.Empty; // Id de l'API externe
    public DateTime LastSyncDateTime { get; set; }

    // Identification de l'offre
    public string JobId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    // Détails
    public string JobDescription { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public string Salary { get; set; } = string.Empty;
    public string RemotePolicy { get; set; } = string.Empty;

    // URLs
    public string DetailUrl { get; set; } = string.Empty;
    public string ApplyLink { get; set; } = string.Empty;

    // Dates
    public DateTime CreatedDate { get; set; }
    public string? PostedDateRaw { get; set; }
    public DateTime? ApplyDate { get; set; }

    // Source & Tracking d'origine
    public int ScrapingSourceId { get; set; }
    public string SourcePlatform { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Stockage JSON pour les données additionnelles
    public string? AdditionalDataJson { get; set; }

    [NotMapped]
    public Dictionary<string, string>? AdditionalData
    {
        get => string.IsNullOrEmpty(AdditionalDataJson)
                ? null
                : JsonSerializer.Deserialize<Dictionary<string, string>>(AdditionalDataJson);
        set => AdditionalDataJson = value == null
                ? null
                : JsonSerializer.Serialize(value);
    }
}
