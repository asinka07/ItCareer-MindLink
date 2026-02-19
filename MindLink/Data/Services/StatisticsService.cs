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

        public async Task<List<DailyMood>> GetDailyMoodCurrentMonthAsync(string userCode)
        {
            var now = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

            var data = await _context.Records
                .Where(r => r.UserCode == userCode && r.RecordDate.Year == now.Year && r.RecordDate.Month == now.Month)
                .GroupBy(r => r.RecordDate.Day)
                .Select(g => new DailyMood
                {
                    Day = g.Key,
                    AverageMood = g.Average(r => r.Rate)
                })
                .ToListAsync();

            var result = new List<DailyMood>();
            for (int day = 1; day <= daysInMonth; day++)
            {
                var existing = data.FirstOrDefault(x => x.Day == day);
                result.Add(new DailyMood
                {
                    Day = day,
                    AverageMood = existing?.AverageMood ?? 0
                });
            }

            return result;
        }

        public async Task<int[]> GetMonthlyMoodCountsAsync(string userCode)
        {
            var now = DateTime.Now;

            var records = await _context.Records
                .Where(r => r.UserCode == userCode && r.RecordDate.Year == now.Year && r.RecordDate.Month == now.Month)
                .ToListAsync();

            int happyCount = records.Count(r => r.Sentiment == "happy");
            int neutralCount = records.Count(r => r.Sentiment == "neutral");
            int sadCount = records.Count(r => r.Sentiment == "sad");

            return new int[] { sadCount, neutralCount, happyCount };
        }

    }
}
