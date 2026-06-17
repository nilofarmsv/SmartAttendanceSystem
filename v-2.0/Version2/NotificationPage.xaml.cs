using System;
using System.Collections.Generic;
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

namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : Page
    {
        private ManageNotificationsViewModel vm;
        public NotificationPage()
        {
            InitializeComponent();
            vm = new ManageNotificationsViewModel();
            DataContext = vm;

            SendNotificationsDataGrid.ItemsSource = vm.Notifications;
        }
        public void Update_Click(object sender, RoutedEventArgs e)
        {
            vm.LoadNotificationsFromDb();
        }
    }
}
