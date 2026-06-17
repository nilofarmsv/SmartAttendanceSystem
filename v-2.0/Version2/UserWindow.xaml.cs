using System;
using System.Collections.Generic;
using System.IO;
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
using Wpf.Ui.Appearance;

namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private string _fullName;
        public UserWindow(string fullName)
        {
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
            _fullName = fullName;
            //labelWelcome_zs.Text = $"خوش آمدید {_fullName}";
            MainFrame.Content = new EmployeeAttendancePage();
            welcome.Text = Healpers.LoggedInUser.Username + " عزیز";
            role.Text = "نقش شما: " + Healpers.LoggedInUser.Role;
            using (MemoryStream ms = new MemoryStream(Healpers.LoggedInUser.Image))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                ProfileImage.Source = bitmap;
            }
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void UserMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EmployeeAttendancePage();
        }

        private void NotificationsMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new NotificationPage();
        }

        private void LeaveMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new LeaveRequestPage();
        }
    }
}
