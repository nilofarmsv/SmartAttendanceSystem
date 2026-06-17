using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSanse.Data;

namespace WorkSanse.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EmployeeItemViewModel> Employees { get; set; }
        private List<EmployeeItemViewModel> AllEmployees;

        public EmployeeViewModel()
        {
            Employees = new ObservableCollection<EmployeeItemViewModel>();
            AllEmployees = new List<EmployeeItemViewModel>();
            LoadUsers();
        }
        

       
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

        public void LoadUsers()
        {
            using (var db = new AppDbContext())
            {
                var usersFromDb = db.Employee.ToList();

                Employees.Clear();
                AllEmployees.Clear();

                foreach (var u in usersFromDb)
                {
                    var employee = new EmployeeItemViewModel
                    {
                        Id=u.Id,
                        Name = u.Name,
                        Family = u.Family,
                        NationalCode = u.NationalCode,
                        Position = u.Position,
                        Phone = u.Phone,
                        UserName = u.UserName,
                        IsActive = u.IsActive ?? false,
                        filePath = u.filePath
                    };

                    Employees.Add(employee);
                    AllEmployees.Add(employee);
                }
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                    OnPropertyChanged(nameof(IsActiveDisplay));
                }
            }
        }

        public string IsActiveDisplay => _isActive ? "فعال" : "غیر فعال";

        private string _position;
        public string Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                    OnPropertyChanged(nameof(PositionDisplay));
                }
            }
        }

        public string PositionDisplay => _position switch
        {
            "Admin" => "مدیر",
            "Employee" => "کارمند",
            _ => _position
        };

        private EmployeeItemViewModel _selectedUser;
        public EmployeeItemViewModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                    OnPropertyChanged(nameof(IsUserSelected));
                }
            }
        }
        private void ApplyFilter()
        {
            var filtered = AllEmployees.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchUserName))
                filtered = filtered.Where(x => x.UserName.Contains(SearchUserName));

            Employees.Clear();
            foreach (var item in filtered)
                Employees.Add(item);
        }
        public bool IsUserSelected => SelectedUser != null;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
