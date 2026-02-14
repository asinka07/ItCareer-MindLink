using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;

namespace MindLink.Data.Services
{
    public class LogsService
    {
        private readonly MindLinkDbContext _context;

        public LogsService(MindLinkDbContext context)
        {
            _context = context;
        }

        public async Task<List<Log>> GetAllLogs()
        {
            return await _context.Log.Include(u => u.User).ToListAsync();
        }

        public async Task CreateLog(User user, string status)
        {
            Log log = new Log();
            log.UserCode = user.UserCode;
            log.Status = status;
            log.Date = DateTime.Now;
            _context.Log.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
