using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkSanse.Healpers
{
    public class PersianDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            DateTime dateTime;

            // پشتیبانی از DateTime?
            if (value is DateTime?)
            {
                var dt = (DateTime?)value;
                if (!dt.HasValue) return "";
                dateTime = dt.Value;
            }
            else
            {
                dateTime = (DateTime)value;
            }

            var pc = new PersianCalendar();

            string persianDate =
                $"{pc.GetYear(dateTime)}/" +
                $"{pc.GetMonth(dateTime):00}/" +
                $"{pc.GetDayOfMonth(dateTime):00}";

            string time = dateTime.ToString("HH:mm");

            return $"{persianDate}  {time}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
