using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;

namespace MindLink.Data.Services
{
    public class DashboardService
    {
        private readonly MindLinkDbContext _context;

        public DashboardService(MindLinkDbContext context)
        {
            _context = context;
        }

        // ── Общо потребители ──
        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        // ── Записи през последните 30 дни ──
        public async Task<int> GetRecordsThisMonthAsync()
        {
            var from = DateTime.Now.AddDays(-30).Date;
            return await _context.Records.CountAsync(r => r.RecordDate.Date >= from);
        }

        // ── Активни ресурси ──
        public async Task<int> GetActiveResourcesAsync()
        {
            return await _context.Resources.CountAsync(r => r.IsVisible);
        }

        // ── Влизания днес ──
        public async Task<int> GetLoginsTodayAsync()
        {
            var today = DateTime.Now.Date;
            return await _context.Log.CountAsync(l => l.Date.Date == today);
        }

        // ── Брой записи по настроение (последните 30 дни) ──
        public async Task<EmotionCountsDto> GetEmotionCountsAsync()
        {
            var from = DateTime.Now.AddDays(-30).Date;
            var records = await _context.Records
                .Where(r => r.RecordDate.Date >= from)
                .ToListAsync();

            return new EmotionCountsDto
            {
                Positive = records.Count(r => r.Sentiment == "positive"),
                Neutral = records.Count(r => r.Sentiment == "neutral"),
                Negative = records.Count(r => r.Sentiment == "negative")
            };
        }

        // ── Активност по дни (последните 7 дни) ──
        public async Task<List<DayActivityDto>> GetWeeklyActivityAsync()
        {
            var from = DateTime.Now.AddDays(-6).Date;

            var grouped = await _context.Records
                .Where(r => r.RecordDate.Date >= from)
                .GroupBy(r => r.RecordDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new List<DayActivityDto>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Now.AddDays(-i).Date;
                var found = grouped.FirstOrDefault(g => g.Date == date);
                result.Add(new DayActivityDto
                {
                    Label = date.ToString("ddd", new System.Globalization.CultureInfo("bg-BG")),
                    Count = found?.Count ?? 0
                });
            }
            return result;
        }

        // ── Най-активни потребители (топ 5, последните 30 дни) ──
        public async Task<List<TopUserDto>> GetTopUsersAsync()
        {
            var from = DateTime.Now.AddDays(-30).Date;

            var grouped = await _context.Records
                .Where(r => r.RecordDate.Date >= from)
                .Include(r => r.User)
                .GroupBy(r => r.UserCode)
                .Select(g => new
                {
                    UserCode = g.Key,
                    Name = g.First().User.Name,
                    RecordCount = g.Count(),
                    TopSentiment = g.GroupBy(r => r.Sentiment)
                                    .OrderByDescending(sg => sg.Count())
                                    .Select(sg => sg.Key)
                                    .FirstOrDefault()
                })
                .OrderByDescending(x => x.RecordCount)
                .Take(5)
                .ToListAsync();

            return grouped.Select(g => new TopUserDto
            {
                Name = g.Name,
                RecordCount = g.RecordCount,
                TopEmotion = g.TopSentiment switch
                {
                    "positive" => "Позитивно",
                    "negative" => "Негативно",
                    _ => "Неутрално"
                }
            }).ToList();
        }

        // ── Последни 5 лога ──
        public async Task<List<RecentLogDto>> GetRecentLogsAsync()
        {
            return await _context.Log
                .OrderByDescending(l => l.Date)
                .Take(5)
                .Select(l => new RecentLogDto
                {
                    UserCode = l.UserCode,
                    Date = l.Date,
                    Status = l.Status
                })
                .ToListAsync();
        }
    }

    // ── DTOs ──
    public class EmotionCountsDto
    {
        public int Positive { get; set; }
        public int Neutral { get; set; }
        public int Negative { get; set; }
        public int Total => Positive + Neutral + Negative;
    }

    public class DayActivityDto
    {
        public string Label { get; set; } = "";
        public int Count { get; set; }
    }

    public class TopUserDto
    {
        public string Name { get; set; } = "";
        public int RecordCount { get; set; }
        public string TopEmotion { get; set; } = "";
    }

    public class RecentLogDto
    {
        public string UserCode { get; set; } = "";
        public DateTime Date { get; set; }
        public string Status { get; set; } = "";
    }
}
