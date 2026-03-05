using System;
using System.Threading.Tasks;

namespace CareerCraft.Core.Services;

public class SyncAllResult
{
    public int TotalProcessed { get; set; }
    public int Added { get; set; }
    public int Updated { get; set; }
    public int Deactivated { get; set; }
    public int Deleted { get; set; }
    public DateTime SyncDate { get; set; } = DateTime.UtcNow;
}

public class SyncByIdResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Added, Updated, NotFound
}

public interface IVacancySyncService
{
    Task<SyncAllResult> SyncAllAsync();
    Task<SyncByIdResult> SyncByIdAsync(int externalId);
}
