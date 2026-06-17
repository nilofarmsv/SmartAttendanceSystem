using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSanse.Data;
using WorkSanse.Healpers;



public class ManageNotificationsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Notification> Notifications { get; set; }
    public List<Notification> AllNotifications { get; set; } = new List<Notification>();

    public enum NotificationFilter
    {
        All,
        Sent,
        NotSent
    }
    private NotificationFilter _selectedFilter = NotificationFilter.All;
    public NotificationFilter SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            _selectedFilter = value;
            ApplyFilter();
            OnPropertyChanged(nameof(SelectedFilter));
        }
    }

    private Notification _selectedNotification;
    public Notification SelectedNotification
    {
        get => _selectedNotification;
        set
        {
            _selectedNotification = value;
            OnPropertyChanged(nameof(SelectedNotification));
        }
    }

    private bool _isSendModalOpen;
    public bool IsSendModalOpen
    {
        get => _isSendModalOpen;
        set
        {
            _isSendModalOpen = value;
            OnPropertyChanged(nameof(IsSendModalOpen));
        }
    }

    private string _notificationText;
    public string NotificationText
    {
        get => _notificationText;
        set
        {
            _notificationText = value;
            OnPropertyChanged(nameof(NotificationText));
        }
    }

    public ManageNotificationsViewModel()
    {
        Notifications = new ObservableCollection<Notification>();
        LoadNotificationsFromDb();
    }

    public void LoadNotificationsFromDb()
    {
        using var db = new AppDbContext();
        var notifFromDb = db.Notifications.OrderByDescending(n => n.SentAt).ToList();

        Notifications.Clear();
        AllNotifications.Clear();

        foreach (var u in notifFromDb)
        {
            var senderRole = db.Employee
                .Where(a => a.Id == u.EmploeeId)
                .Select(a => a.Position)
                .FirstOrDefault();

            Notifications.Add(new Notification
            {
                Id = u.Id,
                Date = u.SentAt ?? DateTime.Now,
                Role = senderRole == "Manegement" ? "مدیریت" :
                       senderRole == "Admin" ? "ادمین" : "سیستم",
                Message = u.Message ?? "Null",
                IsSent = u.IsSent ?? false,
                Status = (u.IsSent ?? false) ? "ارسال شد" : "ارسال نشده"
            });
        }

        // اینجا درست می‌کنیم AllNotifications
        AllNotifications = Notifications.ToList();
    }

    private void ApplyFilter()
    {
        Notifications.Clear();

        IEnumerable<Notification> filtered = AllNotifications;

        if (SelectedFilter == NotificationFilter.Sent)
            filtered = AllNotifications.Where(n => n.IsSent);
        else if (SelectedFilter == NotificationFilter.NotSent)
            filtered = AllNotifications.Where(n => !n.IsSent);

        foreach (var n in filtered)
            Notifications.Add(n);
    }

    public void AddNotificationToDb()
    {
        if (string.IsNullOrWhiteSpace(NotificationText))
            return;

        using var context = new AppDbContext();
        var id = Guid.NewGuid();

        var notifDb = new Notifications
        {
            Id = id,
            EmploeeId = LoggedInUser.UserId,
            Type = "Managment",
            Message = NotificationText,
            IsSent = false,
            SentAt = null
        };

        context.Notifications.Add(notifDb);
        context.SaveChanges();

        Notifications.Add(new Notification
        {
            Id = notifDb.Id,
            Message = notifDb.Message,
            Status = "ارسال نشده",
            Date = DateTime.Now,
            IsSent = false,
            Role = "مدیریت"
        });

        AllNotifications.Add(Notifications.Last());

        NotificationText = "";
        IsSendModalOpen = false;
    }
    public void SortNotification()
    {
        var sort = Notifications.OrderBy(n => n.IsSent).ThenByDescending(n => n.Date).ToList();
        Notifications.Clear();
        foreach (var n in sort)
            Notifications.Add(n);
    }
    public void SendNotification(Notification notif)
    {
        if (notif == null) return;

        using var context = new AppDbContext();
        var notifDb = context.Notifications.FirstOrDefault(n=>n.Id==notif.Id);

        if (notifDb != null)
        {
            notifDb.IsSent = true;
            notifDb.SentAt = DateTime.Now;
            context.SaveChanges();
        }
        notif.IsSent = true;
        notif.Status = "ارسال شد";
        notif.Date = notifDb?.SentAt ?? DateTime.Now;

        SortNotification();
    }

    public void DeleteNotification(Notification notif)
    {
        if (notif == null) return;

        using var context = new AppDbContext();
        var notifDb = context.Notifications.FirstOrDefault(n => n.Id == notif.Id);
        if (notifDb != null)
        {
            context.Notifications.Remove(notifDb);
            context.SaveChanges();
        }

        Notifications.Remove(notif);
        AllNotifications.Remove(notif);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

