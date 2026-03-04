using CareerCraft.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerCraft.Core.Services;

public interface ISkillService
{
    Task<IEnumerable<Skill>> GetAllAsync();
    Task<Skill?> GetByIdAsync(int id);
    Task<Skill> CreateAsync(Skill skill);
    Task UpdateAsync(Skill skill);
    Task DeleteAsync(int id);
}
