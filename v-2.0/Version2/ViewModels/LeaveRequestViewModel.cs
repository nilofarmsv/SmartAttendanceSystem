using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WorkSanse.Data;
using WorkSanse.Healpers;

namespace WorkSanse.ViewModels
{
    public class LeaveRequestViewModel : INotifyPropertyChanged
    {
        private readonly Guid _currentEmployeeId = Healpers.LoggedInUser.UserId;
           

        public DateTime? FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(); }
        }
        private DateTime? _fromDate;

        public DateTime? ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(); }
        }
        private DateTime? _toDate;

        public string Reason
        {
            get => _reason;
            set { _reason = value; OnPropertyChanged(); }
        }
        private string _reason;

        public ObservableCollection<LeaveItemViewModel> MyLeaves { get; }

        public ICommand SubmitLeaveCommand { get; }

        public LeaveRequestViewModel()
        {
            MyLeaves = new ObservableCollection<LeaveItemViewModel>();
            SubmitLeaveCommand = new RelayCommand(_=>SubmitLeave());
            LoadMyLeaves();
        }

        // ✅ ثبت مرخصی
        private void SubmitLeave()
        {
            if (FromDate == null || ToDate == null || string.IsNullOrWhiteSpace(Reason))
            {
                MessageBox.Show("همه فیلدها الزامی هستند");
                return;
            }

            using var db = new AppDbContext();

            var leave = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmploeeId = _currentEmployeeId,
                FormDate = FromDate.Value,
                ToDate = ToDate.Value,
                Text = Reason,
                Status = "0",
                RequestAt = DateTime.Now
            };

            db.LeaveRequest.Add(leave);
            db.SaveChanges();

            MyLeaves.Insert(0, new LeaveItemViewModel
            {
                StartDate = leave.FormDate,
                EndDate = leave.ToDate,
                Text = leave.Text,
                Status = "در انتظار"
            });
            LoadMyLeaves();

            FromDate = null;
            ToDate = null;
            Reason = string.Empty;
        }

        // ✅ لود درخواست‌ها
        private void LoadMyLeaves()
        {
            MyLeaves.Clear();

            using var db = new AppDbContext();

            var leaves = db.LeaveRequest
                .Where(x => x.EmploeeId == _currentEmployeeId)
                .OrderByDescending(x => x.RequestAt)
                .ToList();

            foreach (var x in leaves)
            {
                MyLeaves.Add(new LeaveItemViewModel
                {
                    StartDate = x.FormDate,
                    EndDate = x.ToDate,
                    Text = x.Text,
                    Status = x.Status == "0" ? "در انتظار" :
                             x.Status == "1" ? "تایید شده" : "رد شده"
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}