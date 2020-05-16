using System;

namespace RandREng.Utility
{
    public static class DateExtensions
    {


        public static DateTime NextBusinessDate(this DateTime pDate)
        {
            DateTime dtNextBusinessDay = pDate;
            do
            {
                dtNextBusinessDay = dtNextBusinessDay.AddDays(1);
            } while (dtNextBusinessDay.DayOfWeek == DayOfWeek.Saturday || dtNextBusinessDay.DayOfWeek == DayOfWeek.Sunday);
            return dtNextBusinessDay;
        }

        public static DateTime PreviousBusinessDate(this DateTime pDate)
        {
            DateTime dtPrevBusinessDay = pDate;
            do
            {
                dtPrevBusinessDay = dtPrevBusinessDay.AddDays(-1);
            } while (dtPrevBusinessDay.DayOfWeek == DayOfWeek.Saturday || dtPrevBusinessDay.DayOfWeek == DayOfWeek.Sunday);
            return dtPrevBusinessDay;
        }

    }

}
