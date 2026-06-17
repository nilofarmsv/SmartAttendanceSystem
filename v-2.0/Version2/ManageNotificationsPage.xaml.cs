using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WorkSanse.Data;
using WorkSanse.Healpers;

namespace WorkSanse
{
    public partial class ManageNotificationsPage : Page
    {
        private ManageNotificationsViewModel vm;

        public ManageNotificationsPage()
        {
            InitializeComponent();
            vm = new ManageNotificationsViewModel();
            DataContext = vm;

            NotificationsDataGrid.ItemsSource = vm.Notifications;
        }

        // باز کردن مودال ارسال
        private void OpenSendModal_Click(object sender, RoutedEventArgs e)
        {
            vm.NotificationText = "";
            vm.IsSendModalOpen = true;
        }

        // لغو مودال ارسال
        private void CancelSend_Click(object sender, RoutedEventArgs e)
        {
            vm.IsSendModalOpen = false;
        }

        // تایید مودال ارسال
        private void ConfirmSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(vm.NotificationText))
            {
                MessageBox.Show("متن اعلان را وارد کنید");
                return;
            }

            vm.AddNotificationToDb();
        }

        // ارسال اعلان هر سطر
        private void SendNotification_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var notif = button.DataContext as Notification;

            if (notif != null)
            {
                vm.SendNotification(notif);
            }
        }

        // حذف اعلان هر سطر
        private void DeleteNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var notif = button.DataContext as Notification;

            if (notif != null)
            {
                vm.DeleteNotification(notif);
            }
        }
    }
}