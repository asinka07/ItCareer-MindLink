using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;

namespace MindLink.Data.Services;

public class UsersService
{
    private readonly MindLinkDbContext _context;

    public UsersService(MindLinkDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.Include(u => u.Role).ToListAsync();
    }

    public async Task CreateUser(User user)
    {
        var hasher = new PasswordHasher<object>();
        user.Password = hasher.HashPassword(user, user.Password);
        if (user.Birthday == null)
        {
            user.Birthday = DateTime.Now;
        }
        user.LastLogin = DateTime.Now;
        user.CreatedAt = DateTime.Now;
        user.RoleId = 1;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        var existing = await _context.Users.FindAsync(user.UserCode);
        if (existing == null)
        {
            return;
        }
        existing.Username = user.Username;
        existing.Name = user.Name;
        existing.Gender = user.Gender;
        existing.Birthday = user.Birthday;
        existing.RoleId = user.RoleId;
        await _context.SaveChangesAsync();
    }

    public async Task LogUser(User userInfo)
    {
        var user = await _context.Users.FindAsync(userInfo.UserCode);
        if (user == null)
        {
            return;
        }
        user.LastLogin = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByCode(string code)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserCode == code);
    }

    public async Task UpdatePassword(User userInfo)
    {
        var user = await _context.Users.FindAsync(userInfo.UserCode);
        if (user == null)
        {
            return;
        }
        var hasher = new PasswordHasher<object>();
        user.Password = hasher.HashPassword(null, userInfo.Password);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(User userInfo)
    {
        var user = await _context.Users.FindAsync(userInfo.UserCode);
        if (user == null)
        {
            return;
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public bool IsCodeExisting(string code)
    {
        return _context.Users.Any(u => u.UserCode == code);
    }
}
