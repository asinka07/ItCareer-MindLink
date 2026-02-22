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

        //public async Task<List<LineChartStatistic>> GetDailyMoodCurrentMonthAsync(string userCode)
        //{
        //    var now = DateTime.Now;
        //    int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

        //    var data = await _context.Records
        //        .Where(r => r.UserCode == userCode && r.RecordDate.Year == now.Year && r.RecordDate.Month == now.Month)
        //        .GroupBy(r => r.RecordDate.Day)
        //        .Select(g => new LineChartStatistic
        //        {
        //            DayOrMonth = g.Key,
        //            AverageMood = g.Average(r => r.Rate)
        //        })
        //        .ToListAsync();

        //    var result = new List<LineChartStatistic>();
        //    for (int day = 1; day <= daysInMonth; day++)
        //    {
        //        var existing = data.FirstOrDefault(x => x.DayOrMonth == day);
        //        if (day > data.Max(d => d.DayOrMonth))
        //        {
        //            result.Add(new LineChartStatistic
        //            {
        //                DayOrMonth = day,
        //                AverageMood = existing?.AverageMood ?? null
        //            });
        //        }
        //        else
        //        {
        //            result.Add(new LineChartStatistic
        //            {
        //                DayOrMonth = day,
        //                AverageMood = existing?.AverageMood ?? data.Where(d => d.DayOrMonth <= day).Average(d => d.AverageMood)
        //            });
        //        }
        //    }

        //    return result;
        //}

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



        public async Task<List<LineChartStatistic>> GetMoodTrendAsync(string userCode, StatisticPeriod period)
        {
            var now = DateTime.Now;
            DateTime from, to;

            switch (period)
            {
                case StatisticPeriod.Week:
                    int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                    from = now.AddDays(-diff).Date;
                    to = from.AddDays(6);
                    break;
                case StatisticPeriod.Month:
                    from = new DateTime(now.Year, now.Month, 1);
                    to = from.AddMonths(1).AddDays(-1);
                    break;
                case StatisticPeriod.Year:
                    from = new DateTime(now.Year, 1, 1);
                    to = new DateTime(now.Year, 12, 31);
                    break;
                default:
                    from = now.AddDays(-7);
                    to = now;
                    break;
            }

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
                    if (month > data.Max(d => d.DayOrMonth))
                    {
                        result.Add(new LineChartStatistic
                        {
                            DayOrMonth = month,
                            AverageMood = existing?.AverageMood ?? null,
                            Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month)
                        });
                    }
                    else
                    {
                        result.Add(new LineChartStatistic
                        {
                            DayOrMonth = month,
                            AverageMood = existing?.AverageMood ?? data.Where(d => d.DayOrMonth <= month && d.DayOrMonth >= 1).Average(d => d.AverageMood),
                            Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month)
                        });
                    }
                }
                return result;
            }
            else if(period == StatisticPeriod.Month)
            {
                from = new DateTime(now.Year, now.Month, 1);
                to = from.AddMonths(1).AddDays(-1);

                data = await dataQuery
                    .GroupBy(r => r.RecordDate.Day)
                    .Select(g => new LineChartStatistic
                    {
                        DayOrMonth = g.Key,
                        AverageMood = g.Average(r => r.Rate)
                    }).ToListAsync();

                int totalDays = DateTime.DaysInMonth(now.Year, now.Month);
                var result = new List<LineChartStatistic>();
                for (int i = 0; i < totalDays; i++)
                {
                    var date = from.AddDays(i);
                    var existing = data.FirstOrDefault(d => d.DayOrMonth == date.Day);
                    result.Add(new LineChartStatistic
                    {
                        DayOrMonth = date.Day,
                        AverageMood = existing?.AverageMood ?? data.Where(d => d.DayOrMonth <= date.Day).Average(d => d.AverageMood),
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
                        AverageMood = existing?.AverageMood ?? data.Where(d => d.DayOrMonth <= date.Day).Average(d => d.AverageMood),
                        Label = date.ToString("ddd", CultureInfo.CurrentCulture)
                    });
                }

                return result;
            }

        }

    }
}