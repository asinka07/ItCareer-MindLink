using MindLink.Data.Enums;
using System;
using System.Globalization;

namespace MindLink.Data.Helpers
{
    public class PeriodNavigator
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public StatisticPeriod Period { get; private set; }

        public bool IsPrevDisabled => Start <= MinDate;
        public bool IsNextDisabled => End >= MaxDate;

        public PeriodNavigator(StatisticPeriod period, DateTime minDate, DateTime maxDate)
        {
            Period = period;
            MinDate = minDate;
            MaxDate = maxDate;
            SetToToday();
        }

        public void SetPeriod(StatisticPeriod period)
        {
            Period = period;
            SetToToday();
        }

        public void Prev()
        {
            switch (Period)
            {
                case StatisticPeriod.Week:
                    Start = Start.AddDays(-7);
                    if (Start < MinDate) Start = MinDate;
                    End = Start.AddDays(6);
                    break;
                case StatisticPeriod.Month:
                    Start = Start.AddMonths(-1);
                    if (Start < MinDate) Start = new DateTime(MinDate.Year, MinDate.Month, 1);
                    End = Start.AddMonths(1).AddDays(-1);
                    break;
                case StatisticPeriod.Year:
                    Start = Start.AddYears(-1);
                    if (Start < MinDate) Start = new DateTime(MinDate.Year, 1, 1);
                    End = new DateTime(Start.Year, 12, 31);
                    break;
            }
        }

        public void Next()
        {
            switch (Period)
            {
                case StatisticPeriod.Week:
                    Start = Start.AddDays(7);
                    if (Start > MaxDate) Start = MaxDate.AddDays(-6);
                    End = Start.AddDays(6);
                    break;
                case StatisticPeriod.Month:
                    Start = Start.AddMonths(1);
                    if (Start > MaxDate) Start = new DateTime(MaxDate.Year, MaxDate.Month, 1);
                    End = Start.AddMonths(1).AddDays(-1);
                    break;
                case StatisticPeriod.Year:
                    Start = Start.AddYears(1);
                    if (Start > MaxDate) Start = new DateTime(MaxDate.Year, 1, 1);
                    End = new DateTime(Start.Year, 12, 31);
                    break;
            }
        }

        public void SetToToday()
        {
            var now = DateTime.Now;
            switch (Period)
            {
                case StatisticPeriod.Week:
                    int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                    Start = now.AddDays(-diff).Date;
                    End = Start.AddDays(6);
                    break;
                case StatisticPeriod.Month:
                    Start = new DateTime(now.Year, now.Month, 1);
                    End = Start.AddMonths(1).AddDays(-1);
                    break;
                case StatisticPeriod.Year:
                    Start = new DateTime(now.Year, 1, 1);
                    End = new DateTime(now.Year, 12, 31);
                    break;
            }
        }

        public string GetLabel()
        {
            switch (Period)
            {
                case StatisticPeriod.Week:
                    return (Start.Year != End.Year || Start.Year != DateTime.Now.Year)
                        ? $"{Start:dd.MM.yyyy} - {End:dd.MM.yyyy}"
                        : $"{Start:dd.MM} - {End:dd.MM}";
                case StatisticPeriod.Month:
                    return Start.Year != DateTime.Now.Year
                        ? Start.ToString("MMMM yyyy", CultureInfo.CurrentCulture)
                        : Start.ToString("MMMM", CultureInfo.CurrentCulture);
                case StatisticPeriod.Year:
                    return Start.Year.ToString();
                default:
                    return "";
            }
        }
    }
}