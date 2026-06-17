using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WorkSanse.Healpers
{
    public static class PersianDateHelper
    {

        public static string GetPersianDayName(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Saturday => "شنبه",
                DayOfWeek.Sunday => "یکشنبه",
                DayOfWeek.Monday => "دوشنبه",
                DayOfWeek.Tuesday => "سه‌شنبه",
                DayOfWeek.Wednesday => "چهارشنبه",
                DayOfWeek.Thursday => "پنجشنبه",
                DayOfWeek.Friday => "جمعه",
                _ => ""
            };
        }
    }

}
