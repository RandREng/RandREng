using RandREng.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Testing.RandREng.Utilities
{

    public class DateTimeExtTests
    {
        [Fact]
        public void NextBusinessDate()
        {
            for (int i = 0; i < 7; i++)
            {
                DateTime test = DateTime.Today.AddDays(i);
                Assert.Equal(NextBussines(test), test.NextBusinessDate().DayOfWeek);
            }
        }

        [Fact]
        public void PrefBusinessDate()
        {
            for (int i = 0; i < 7; i++)
            {
                DateTime test = DateTime.Today.AddDays(i);
                Assert.Equal(PreviousBussines(test), test.PreviousBusinessDate().DayOfWeek);
            }
        }

        public DayOfWeek NextBussines(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Monday => DayOfWeek.Tuesday,
                DayOfWeek.Tuesday => DayOfWeek.Wednesday,
                DayOfWeek.Wednesday => DayOfWeek.Thursday,
                DayOfWeek.Thursday => DayOfWeek.Friday,
                _ => DayOfWeek.Monday,
            };
        }

        public DayOfWeek PreviousBussines(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Tuesday => DayOfWeek.Monday,
                DayOfWeek.Wednesday => DayOfWeek.Tuesday,
                DayOfWeek.Thursday => DayOfWeek.Wednesday,
                DayOfWeek.Friday => DayOfWeek.Thursday,
                _ => DayOfWeek.Friday,
            };
        }
    }
}
