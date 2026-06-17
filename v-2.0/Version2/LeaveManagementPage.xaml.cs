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
using WorkSanse.ViewModels;

namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for LeaveManagementPage.xaml
    /// </summary>
    public partial class LeaveManagementPage : Page
    {
        private ManageLeaveViewModel vm;
        public LeaveManagementPage()
        {
            InitializeComponent();
            vm = new ManageLeaveViewModel();
            DataContext = vm;

            LeaveManagementDataGrid.ItemsSource = vm.LeaveRequests;
        }
        private void ApproveLeave_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ManageLeaveViewModel;
            var item = (sender as Button)?.DataContext as LeaveItemViewModel;
            vm.ApproveLeave(item);
        }

        private void RejectLeave_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ManageLeaveViewModel;
            var item = (sender as Button)?.DataContext as LeaveItemViewModel;
            vm.RejectLeave(item);
        }
    }
}
