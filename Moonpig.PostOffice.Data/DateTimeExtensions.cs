namespace Moonpig.PostOffice.Data
{
    using System;

    public static class DateTimeExtensions 
    {
        public static DateTime AddWorkingDays(this DateTime date, int days)
        {
            for (var i = 0; i < days; i++)
            {
                date = date.NextWorkingDay();
            }

            return date;
        }

        public static DateTime NextWorkingDay(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    return date.AddDays(3);
                case DayOfWeek.Saturday:
                    return date.AddDays(3);
                case DayOfWeek.Sunday:
                    return date.AddDays(2);
                default:
                    return date.AddDays(1);
            }
        }
    }
}