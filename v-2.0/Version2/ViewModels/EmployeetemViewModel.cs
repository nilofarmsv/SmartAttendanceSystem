using System;
using System.ComponentModel;

namespace WorkSanse.ViewModels
{
    public class EmployeeItemViewModel : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public long? TelegramId { get; set; }
        public string Name { get; set; } = null!;
        public string Family { get; set; } = null!;
        public string NationalCode { get; set; } = null!;
        public string? Phone { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Position { get; set; } = null!;
        public byte[] filePath { get; set; }
        public DateTime? HireDate { get; set; }
        public bool? IsActive { get; set; }


        private string _status;
        //public string IsActive
        //{
        //    get => _status;
        //    set
        //    {
        //        _status = value;
        //        PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(nameof(IsActive)));
        //        OnPropertyChanged(nameof(IsActive));
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}