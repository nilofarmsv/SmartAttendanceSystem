using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSanse.Healpers
{
   
        public class Notification : INotifyPropertyChanged
        {
            private string _message;
            private string _Role;
            private bool _IsSent;
            private string _status; // ارسال شده یا نشده
            private DateTime _date;
            private Guid _id;

            public string Role
        {
                get => _Role;
                set { _Role = value; OnPropertyChanged(nameof(Role)); }
            }
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }
        public bool IsSent
        {
            get => _IsSent;
            set { _IsSent = value; OnPropertyChanged(nameof(IsSent)); }
        }

        public string Status
            {
                get => _status;
                set { _status = value; OnPropertyChanged(nameof(Status)); }
            }

            public DateTime Date
            {
                get => _date;
                set { _date = value; OnPropertyChanged(nameof(Date)); }
            }
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    
}
