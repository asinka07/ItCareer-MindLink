using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;

namespace MindLink.Data.Services;

public class RolesService
{
    private readonly MindLinkDbContext _context;

    public RolesService(MindLinkDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllRoles()
    {
        return await _context.Roles.OrderBy(r => r.Id).ToListAsync();
    }
}
