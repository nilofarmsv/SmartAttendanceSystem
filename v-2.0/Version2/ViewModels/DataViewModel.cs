using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace WorkSanse.ViewModels
{
    public class DataViewModel : INotifyPropertyChanged
    {
        private string _persianDateTime;
        public string PersianDateTime
        {
            get => _persianDateTime;
            set
            {
                _persianDateTime = value;
                OnPropertyChanged(nameof(PersianDateTime));
            }
        }

        private DispatcherTimer _timer;

        public DataViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => UpdateDateTime();
            _timer.Start();

            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            var pc = new PersianCalendar();
            var now = DateTime.Now;

            PersianDateTime =
                $"{GetPersianDayName(now.DayOfWeek)} " +
                $"{pc.GetDayOfMonth(now)} " +
                $"{GetPersianMonthName(pc.GetMonth(now))} {pc.GetYear(now)}  " +
                $"{now:HH:mm:ss}";
        }

        private string GetPersianDayName(DayOfWeek day)
        {
            return day switch
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

        private string GetPersianMonthName(int month)
        {
            string[] months =
            {
            "فروردین","اردیبهشت","خرداد","تیر","مرداد","شهریور",
            "مهر","آبان","آذر","دی","بهمن","اسفند"
        };
            return months[month - 1];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

