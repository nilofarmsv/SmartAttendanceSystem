using System;
using System.ComponentModel;

namespace WorkSanse.ViewModels
{
    public class LeaveItemViewModel : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Text { get; set; }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(nameof(Status)));
                OnPropertyChanged(nameof(Status));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}