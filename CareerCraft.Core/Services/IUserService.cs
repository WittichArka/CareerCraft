using CareerCraft.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerCraft.Core.Services;

public interface IUserService
{
    // User CRUD
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);

    // UserInfo CRUD (scoped to user)
    Task<IEnumerable<UserInfo>> GetInfosByUserIdAsync(int userId);
    Task<UserInfo?> GetInfoByIdAsync(int id);
    Task<UserInfo> AddInfoAsync(int userId, UserInfo info);
    Task UpdateInfoAsync(UserInfo info);
    Task DeleteInfoAsync(int id);
    Task ReorderInfosAsync(int userId, List<int> infoIds);
}
