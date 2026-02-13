using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;

namespace MindLink.Data.Services;

public class RecordsService
{
    private readonly MindLinkDbContext _context;

    public RecordsService(MindLinkDbContext context)
    {
        _context = context;
    }
    public async Task<List<Record>> GetAllRecordsByUser(string userCode)
    {
        return await _context.Records.Where(r => r.UserCode == userCode).OrderByDescending(r => r.RecordDate)
                                     .ToListAsync();
    }
    public async Task CreateRecord(Record record)
    {
        record.RecordDate = DateTime.Now;
        _context.Records.Add(record);
        await _context.SaveChangesAsync();
    }
}
