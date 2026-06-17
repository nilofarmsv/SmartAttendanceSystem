using System.IO;
using System.Text;
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
using Wpf.Ui.Controls;

namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _fullName;
        public MainWindow(string fullName)
        {
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
            _fullName = fullName;
            //labelWelcome_zs.Text = $"خوش آمدید {_fullName}";
            MainFrame.Content = new DashboardPage();
            welcome.Text = Healpers.LoggedInUser.Username + " عزیز";
            if (Healpers.LoggedInUser.Role== "Admin")
            {
                role.Text = "نقش شما: ادمین";
            }
            else if (Healpers.LoggedInUser.Role== "Manegement")
            {
                role.Text = "نقش شما: مدیر" ;
            }
            else
            {
                role.Text = "نقش شما: کارمند";
            }
            
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
        private void MainForm_Load(object sender, EventArgs e)
        {
           
           
        }
        public void DashboardMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new DashboardPage() ;
        }

        public void UserMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new UserManagementPage();
        }

        public void NotificationsMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ManageNotificationsPage();
        }
        public void Notifications_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ManageNotificationsPage();
        }

        public void AttendanceMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AttendanceManagementPage();
        }
        public void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void LeaveMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new LeaveManagementPage();
        }

        public void ReportingMenu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ReportingPage();
        }
        private Border _selectedBorder;

        private void MenuBorder_Click(object sender, MouseButtonEventArgs e)
        {
            if (_selectedBorder != null)
            {
                _selectedBorder.Background = Brushes.Transparent; // ریست کردن بوردر قبلی
            }

            _selectedBorder = sender as Border;
            if (_selectedBorder != null)
            {
                _selectedBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B4A2")); // بوردر فعال
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}