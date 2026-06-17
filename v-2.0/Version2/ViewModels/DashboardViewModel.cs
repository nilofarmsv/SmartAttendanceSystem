using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WorkSanse.Data;
using WorkSanse.Healpers;
using Wpf.Ui.Input;

public class DashboardViewModel : INotifyPropertyChanged
{
    private SeriesCollection _seriesCollection;
    public SeriesCollection SeriesCollection
    {
        get => _seriesCollection;
        set { _seriesCollection = value; OnPropertyChanged(nameof(SeriesCollection)); }
    }

    private string[] _weekDays;
    public string[] WeekDays
    {
        get => _weekDays;
        set { _weekDays = value; OnPropertyChanged(nameof(WeekDays)); }
    }

    private int _totalEmployees;
    public int TotalEmployees
    {
        get => _totalEmployees;
        set { _totalEmployees = value; OnPropertyChanged(nameof(TotalEmployees)); }
    }
    private int _presentCount;
    public int PresentCount
    {
        get => _presentCount;
        set
        {
            _presentCount = value;
            OnPropertyChanged(nameof(PresentCount));
        }
    }

    private int _absentCount;
    public int AbsentCount
    {
        get => _absentCount;
        set
        {
            _absentCount = value;
            OnPropertyChanged(nameof(AbsentCount));
        }
    }

    private int _leaveCount;
    public int LeaveCount
    {
        get => _leaveCount;
        set
        {
            _leaveCount = value;
            OnPropertyChanged(nameof(LeaveCount));
        }
    }

    private void LoadDailyAttendanceStats(DateTime date)
    {
        using (var db = new AppDbContext())
        {
            var records = db.Attendance
                .Where(a => a.Date.Date == date.Date).ToList();





            PresentCount = records
                .Count(a => a.CheckInTime != null);

            LeaveCount = records
                .Count(a => a.Type == "Leave");

            AbsentCount = TotalEmployees - (PresentCount + LeaveCount);

            if (AbsentCount < 0)
                AbsentCount = 0;
        }
    }



    public DateTime SelectedWeekStart { get; set; }

    public ICommand NextWeekCommand { get; set; }
    public ICommand PrevWeekCommand { get; set; }

    private void LoadPresentCount()
    {
        using var db = new AppDbContext();

        var today = DateTime.Today;

        PresentCount = db.Attendance
            .Count(a =>
                a.Date == today &&
                a.CheckInTime != null &&
                a.CheckOutTime == null);
    }
    public DashboardViewModel()
    {
        SetCurrentWeekStart();

        PrevWeekCommand = new RelayCommand(_=>
        {
            SelectedWeekStart = SelectedWeekStart.AddDays(-7);
            LoadWeeklyAttendance();
            LoadDailyAttendanceStats(SelectedWeekStart);
        });

        NextWeekCommand = new RelayCommand(_ =>
        {
            SelectedWeekStart = SelectedWeekStart.AddDays(7);
            LoadWeeklyAttendance();
            LoadDailyAttendanceStats(SelectedWeekStart);
        });
        LoadTotalEmployees();
        LoadDashboardData();
        LoadWeeklyAttendance();
        LoadPresentCount();
        LoadLeaveCount();
        LoadDailyAttendanceStats(SelectedWeekStart);

    }
    private int _pendingLeaveCount;
    public int PendingLeaveCount
    {
        get => _pendingLeaveCount;
        set
        {
            _pendingLeaveCount = value;
            OnPropertyChanged(nameof(PendingLeaveCount));
        }
    }
    public void LoadDashboardData()
    {
        using var db = new AppDbContext();

        PendingLeaveCount = db.LeaveRequest
            .Count(x => x.Status == "0"); // در انتظار
    }
    private void SetCurrentWeekStart()
    {
        DateTime today = DateTime.Today;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Saturday)) % 7;
        SelectedWeekStart = today.AddDays(-diff); // شنبه هفته جاری
    }

    private bool IsHoliday(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Friday;
    }

    private void LoadWeeklyAttendance()
    {
        WeekDays = new string[7];
        var values = new ChartValues<int>();

        for (int i = 0; i < 7; i++)
        {
            var date = SelectedWeekStart.AddDays(i);
            WeekDays[i] = PersianDateHelper.GetPersianDayName(date);

            if (IsHoliday(date))
            {
                values.Add(0);
                continue;
            }

            values.Add(GetPresentEmployeesCount(date));
        }

        SeriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Title = "افراد حاضر در هفته",
                Values = values,
                PointGeometrySize = 10
            }
        };
    }

    private int GetPresentEmployeesCount(DateTime date)
    {
        using (var db = new AppDbContext())
        {
            //var position=db.Employee.Select(b => b.Position && b.Id);
            // فرض: حضور = CheckInTime != null
            return db.Attendance
                     .Where(a => a.Date.Date == date.Date && a.CheckInTime != null  )
                     .Select(a => a.EmployeeId)
                     .Distinct()
                     .Count();
        }
    }

    private void LoadTotalEmployees()
    {
        using (var db = new AppDbContext())
        {
            TotalEmployees = db.Employee.Count(); // جدول Employee فرض شده
        }
    }
    private void LoadLeaveCount()
    {
        using (var db = new AppDbContext())
        {
            LeaveCount = db.LeaveRequest.Count(); // جدول LeaveRequest فرض شده
        }
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
