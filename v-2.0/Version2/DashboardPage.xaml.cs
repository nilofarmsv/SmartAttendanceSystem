using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkSanse.Data;
using WorkSanse.Healpers;

namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
           
        }
        private int GetPresentEmployeesCount(DateTime date)
        {
            using (var db = new AppDbContext())
            {
                return db.Attendance
                    .Where(a =>
                        a.Date.Date == date.Date &&
                        a.CheckInTime != null
                    )
                    .Select(a => a.EmployeeId)
                    .Distinct()
                    .Count();
            }
        }
       




        //public ObservableCollection<AttendanceChartModel> WeeklyAttendance { get; set; }

        //public void LoadWeeklyData()
        //{
        //    WeeklyAttendance = new ObservableCollection<AttendanceChartModel>();

        //    for (int i = 0; i < 7; i++)
        //    {
        //        var date = DateTime.Today.AddDays(-6 + i);

        //        WeeklyAttendance.Add(new AttendanceChartModel
        //        {
        //            Day = PersianDayOfWeek(date),
        //            Hours = new Random().Next(4, 9) // تستی
        //        });
        //    }
        //}

    }
}
