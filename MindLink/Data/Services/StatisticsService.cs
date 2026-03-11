using Microsoft.EntityFrameworkCore;
using MindLink.Data.Enums;
using MindLink.Data.Models;
using MindLink.Data.ViewModels;
using System.Globalization;

namespace MindLink.Data.Services
{
    public class StatisticsService
    {
        private readonly MindLinkDbContext _context;

        public StatisticsService(MindLinkDbContext context)
        {
            _context = context;
        }

        public async Task<Record?> GetFirstRecordDateAsync(string userCode)
        {
            return await _context.Records.Where(r => r.UserCode == userCode).OrderBy(r => r.RecordDate).FirstOrDefaultAsync();
        }

        public async Task<Record?> GetLastRecordDateAsync(string userCode)
        {
            return await _context.Records.Where(r => r.UserCode == userCode).OrderByDescending(r => r.RecordDate).FirstOrDefaultAsync();
        }


        public async Task<int[]> GetMonthlyMoodCountsAsync(string userCode, StatisticPeriod period, DateTime start, DateTime end)
        {
            var now = DateTime.Now;

            DateTime from = start.Date;
            DateTime to = end.Date;

            var records = await _context.Records
                .Where(r => r.UserCode == userCode && r.RecordDate.Date >= from && r.RecordDate.Date <= to)
                .ToListAsync();

            int happyCount = records.Count(r => r.Sentiment == "positive");
            int neutralCount = records.Count(r => r.Sentiment == "neutral");
            int sadCount = records.Count(r => r.Sentiment == "negative");

            return new int[] { happyCount, neutralCount, sadCount };
        }



        public async Task<List<LineChartStatistic>> GetMoodTrendAsync(string userCode, StatisticPeriod period, DateTime start, DateTime end)
        {
            var now = DateTime.Now;

            DateTime from = start.Date;
            DateTime to = end.Date;

            var dataQuery = _context.Records
                .Where(r => r.UserCode == userCode && r.RecordDate.Date >= from && r.RecordDate.Date <= to);

            List<LineChartStatistic> data;

            if (period == StatisticPeriod.Year)
            {
                data = await dataQuery
                    .GroupBy(r => r.RecordDate.Month)
                    .Select(g => new LineChartStatistic
                    {
                        DayOrMonth = g.Key,
                        AverageMood = g.Average(r => r.Rate)
                    }).ToListAsync();

                var result = new List<LineChartStatistic>();
                for (int month = 1; month <= 12; month++)
                {
                    var existing = data.FirstOrDefault(d => d.DayOrMonth == month);

                        result.Add(new LineChartStatistic
                        {
                            DayOrMonth = month,
                            AverageMood = existing?.AverageMood ?? null,
                            Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month)
                        });

                }
                return result;
            }
            else if (period == StatisticPeriod.Month)
            {
                int totalDays = DateTime.DaysInMonth(from.Year, from.Month);

                data = await dataQuery
                    .GroupBy(r => r.RecordDate.Day)
                    .Select(g => new LineChartStatistic
                    {
                        DayOrMonth = g.Key,
                        AverageMood = g.Average(r => r.Rate)
                    }).ToListAsync();

                var result = new List<LineChartStatistic>();
                for (int i = 0; i < totalDays; i++)
                {
                    var date = from.AddDays(i);
                    var existing = data.FirstOrDefault(d => d.DayOrMonth == date.Day);
                    result.Add(new LineChartStatistic
                    {
                        DayOrMonth = date.Day,
                        AverageMood = existing?.AverageMood ?? null,
                        Label = date.Day.ToString()
                    });
                }
                return result;
            }
            else
            {
                dataQuery = _context.Records
                    .Where(r => r.UserCode == userCode && r.RecordDate.Date >= from && r.RecordDate.Date <= to);

                data = await dataQuery
                    .GroupBy(r => r.RecordDate.Date)
                    .Select(g => new LineChartStatistic
                    {
                        DayOrMonth = g.Key.Day,
                        AverageMood = g.Average(r => r.Rate),
                        Label = g.Key.ToString("ddd", CultureInfo.CurrentCulture)
                    }).ToListAsync();

                int totalDays = (int)(to - from).TotalDays + 1;
                var result = new List<LineChartStatistic>();

                for (int i = 0; i < totalDays; i++)
                {
                    var date = from.AddDays(i);
                    var existing = data.FirstOrDefault(d => d.DayOrMonth == date.Day);
                    result.Add(new LineChartStatistic
                    {
                        DayOrMonth = date.Day,
                        AverageMood = existing?.AverageMood ?? null,
                        Label = date.ToString("ddd", CultureInfo.CurrentCulture)
                    });
                }

                return result;
            }

        }

        public async Task<List<BarChartStatistic>> GetActivityStatsAsync(string userCode, StatisticPeriod period, DateTime start, DateTime end)
        {
            DateTime from = start.Date;
            DateTime to = end.Date;

            var dataQuery = _context.Records
                .Where(r => r.UserCode == userCode && r.RecordDate.Date >= from && r.RecordDate.Date <= to);

            List<BarChartStatistic> data;

            if (period == StatisticPeriod.Year)
            {
                data = await dataQuery
                    .GroupBy(r => r.RecordDate.Month)
                    .Select(g => new BarChartStatistic
                    {
                        DayOrMonth = g.Key,
                        CountOfRecords = g.Count() 
                    })
                    .ToListAsync();

                var result = new List<BarChartStatistic>();
                for (int month = 1; month <= 12; month++)
                {
                    var existing = data.FirstOrDefault(d => d.DayOrMonth == month);
                    result.Add(new BarChartStatistic
                    {
                        DayOrMonth = month,
                        CountOfRecords = existing?.CountOfRecords ?? 0,
                        Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month)
                    });
                }
                return result;
            }
            else if (period == StatisticPeriod.Month)
            {
                int totalDays = DateTime.DaysInMonth(from.Year, from.Month);

                data = await dataQuery
                    .GroupBy(r => r.RecordDate.Day)
                    .Select(g => new BarChartStatistic
                    {
                        DayOrMonth = g.Key,
                        CountOfRecords = g.Count()
                    })
                    .ToListAsync();

                var result = new List<BarChartStatistic>();
                for (int i = 0; i < totalDays; i++)
                {
                    var date = from.AddDays(i);
                    var existing = data.FirstOrDefault(d => d.DayOrMonth == date.Day);
                    result.Add(new BarChartStatistic
                    {
                        DayOrMonth = date.Day,
                        CountOfRecords = existing?.CountOfRecords ?? 0,
                        Label = date.Day.ToString()
                    });
                }
                return result;
            }
            else
            {
                data = await dataQuery
                    .GroupBy(r => r.RecordDate.Date)
                    .Select(g => new BarChartStatistic
                    {
                        DayOrMonth = g.Key.Day,
                        CountOfRecords = g.Count(),
                        Label = g.Key.ToString("ddd", CultureInfo.CurrentCulture)
                    })
                    .ToListAsync();

                int totalDays = (int)(to - from).TotalDays + 1;
                var result = new List<BarChartStatistic>();

                for (int i = 0; i < totalDays; i++)
                {
                    var date = from.AddDays(i);
                    var existing = data.FirstOrDefault(d => d.DayOrMonth == date.Day);
                    result.Add(new BarChartStatistic
                    {
                        DayOrMonth = date.Day,
                        CountOfRecords = existing?.CountOfRecords ?? 0,
                        Label = date.ToString("ddd", CultureInfo.CurrentCulture)
                    });
                }

                return result;
            }
        }

    }
}