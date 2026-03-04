using CareerCraft.Core.Entities;
using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerCraft.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.Include(u => u.UserInfos).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.UserInfos.OrderBy(ui => ui.Order))
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<UserInfo>> GetInfosByUserIdAsync(int userId)
    {
        return await _context.UserInfos
            .Where(ui => ui.UserId == userId)
            .OrderBy(ui => ui.Order)
            .ToListAsync();
    }

    public async Task<UserInfo?> GetInfoByIdAsync(int id)
    {
        return await _context.UserInfos.FindAsync(id);
    }

    public async Task<UserInfo> AddInfoAsync(int userId, UserInfo info)
    {
        info.UserId = userId;
        // Si l'ordre n'est pas spécifié, on l'ajoute à la fin
        if (info.Order == 0)
        {
            var maxOrder = await _context.UserInfos
                .Where(ui => ui.UserId == userId)
                .Select(ui => (int?)ui.Order)
                .MaxAsync() ?? 0;
            info.Order = maxOrder + 1;
        }
        _context.UserInfos.Add(info);
        await _context.SaveChangesAsync();
        return info;
    }

    public async Task UpdateInfoAsync(UserInfo info)
    {
        _context.Entry(info).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteInfoAsync(int id)
    {
        var info = await _context.UserInfos.FindAsync(id);
        if (info != null)
        {
            _context.UserInfos.Remove(info);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ReorderInfosAsync(int userId, List<int> infoIds)
    {
        var infos = await _context.UserInfos
            .Where(ui => ui.UserId == userId)
            .ToListAsync();

        for (int i = 0; i < infoIds.Count; i++)
        {
            var info = infos.FirstOrDefault(ui => ui.Id == infoIds[i]);
            if (info != null)
            {
                info.Order = i + 1;
            }
        }

        await _context.SaveChangesAsync();
    }
}
