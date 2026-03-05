using System;
using System.Collections.Generic;

namespace CareerCraft.Core.Entities;

public class ExternalVacancy
{
    public int Id { get; set; }
    public string JobId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public string Salary { get; set; } = string.Empty;
    public string RemotePolicy { get; set; } = string.Empty;
    public string DetailUrl { get; set; } = string.Empty;
    public string ApplyLink { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? PostedDateRaw { get; set; }
    public DateTime? ApplyDate { get; set; }
    public int ScrapingSourceId { get; set; }
    public string SourcePlatform { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? AdditionalDataJson { get; set; }
    public Dictionary<string, string>? AdditionalData { get; set; }
}
