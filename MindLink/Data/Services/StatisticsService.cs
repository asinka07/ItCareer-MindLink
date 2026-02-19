using Microsoft.EntityFrameworkCore;
using MindLink.Data.ViewModels;
using MindLink.Data.Models;

namespace MindLink.Data.Services
{
    public class StatisticsService
    {
        private readonly MindLinkDbContext _context;

        public StatisticsService(MindLinkDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyStatistics>> GetMonthlyStatsAsync(string userCode)
        {
            var data = await _context.Records
                .Where(r => r.UserCode == userCode)
                .Where(r => r.RecordDate.Year == DateTime.Now.Year)
                .GroupBy(r => r.RecordDate.Month)
                .OrderBy(g => g.Key)
                .Select(g => new MonthlyStatistics
                {
                    MonthId = g.Key,
                    CountOfRecords = g.Count()
                })
                .ToListAsync();

            var result = new List<MonthlyStatistics>();

            for (int month = 1; month <= 12; month++)
            {
                var existing = data.FirstOrDefault(x => x.MonthId == month);

                result.Add(new MonthlyStatistics
                {
                    MonthId = month,
                    CountOfRecords = existing?.CountOfRecords ?? 0
                });
            }

            return result;
        }
    }
}
