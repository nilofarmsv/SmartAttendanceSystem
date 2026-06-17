using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSanse.Data;

namespace WorkSanse.ViewModels
{
    public class ManageLeaveViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<LeaveItemViewModel> LeaveRequests { get; set; }
        private List<LeaveItemViewModel> AllLeaveRequests;

        private string _searchUserName;
        public string SearchUserName
        {
            get => _searchUserName;
            set
            {
                _searchUserName = value;
                ApplyFilter();
                OnPropertyChanged(nameof(SearchUserName));
            }
        }

        public ManageLeaveViewModel()
        {
            LeaveRequests = new ObservableCollection<LeaveItemViewModel>();
            AllLeaveRequests = new List<LeaveItemViewModel>();
            LoadFromDb();   
        }

        // 🔹 لود دیتابیس
        public void LoadFromDb()
        {
            using var db = new AppDbContext();

            var data = db.LeaveRequest
                .Join(db.Employee,
                      l => l.EmploeeId,
                      u => u.Id,
                      (l, u) => new LeaveItemViewModel
                      {
                          Id = l.Id,
                          UserName = u.Name,
                          Text=l.Text,
                          StartDate = l.FormDate,
                          EndDate = l.ToDate,
                          Status = l.Status == "0" ? "در انتظار" :
                                   l.Status == "1" ? "تایید شده" :
                                   "رد شده"
                      })
                .ToList();

            AllLeaveRequests = data;
            ApplySort();
        }

        private void ApplySort()
        {
            var sorted = AllLeaveRequests
                .OrderBy(x => x.Status == "در انتظار" ? 0 : 1)
                .ThenByDescending(x => x.StartDate)
                .ToList();

            LeaveRequests.Clear();
            foreach (var item in sorted)
                LeaveRequests.Add(item);
        }

        private void ApplyFilter()
        {
            var filtered = AllLeaveRequests.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchUserName))
                filtered = filtered.Where(x => x.UserName.Contains(SearchUserName));

            LeaveRequests.Clear();
            foreach (var item in filtered)
                LeaveRequests.Add(item);
        }

        public void ApproveLeave(LeaveItemViewModel item)
        {
            if (item == null) return;

            using var db = new AppDbContext();
            var leave = db.LeaveRequest.FirstOrDefault(x => x.Id == item.Id);
            if (leave == null) return;

            leave.Status = "1";
            db.SaveChanges();

            item.Status = "تایید شده";
            ApplySort();
        }

        public void RejectLeave(LeaveItemViewModel  item)
        {
            if (item == null) return;

            using var db = new AppDbContext();
            var leave = db.LeaveRequest.FirstOrDefault(x => x.Id == item.Id);
            if (leave == null) return;

            leave.Status = "2";
            db.SaveChanges();

            item.Status = "رد شده";
            ApplySort();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}