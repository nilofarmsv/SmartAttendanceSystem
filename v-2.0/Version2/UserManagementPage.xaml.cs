using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WorkSanse.Data;
using WorkSanse.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WorkSanse
{

    public partial class UserManagementPage : Page
    {
        private EmployeeViewModel vm;
        public UserManagementPage()
        {
            InitializeComponent();
            vm = new EmployeeViewModel();
            DataContext = vm;

            EmployeeManagementDataGrid.ItemsSource = vm.Employees;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Rigister fp = new Rigister();
            fp.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //if (EmployeeViewModel.SelectedUser != null)
            //{
            //    EditUser editWindow = new EditUser();
            //    // DataContext فرم ویرایش رو ست می‌کنیم
            //    editWindow.DataContext = EmployeeViewModel.SelectedUser;
            //    editWindow.ShowDialog();
            //}
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;

            if (vm == null || vm.SelectedUser == null)
            {
                MessageBox.Show("لطفاً یک کاربر انتخاب کنید");
                return;
            }

            EditUser editForm = new EditUser(vm.SelectedUser);
            editForm.ShowDialog();
        }

        private void Rigister_Click(object sender, RoutedEventArgs e)
        {

            Rigister fp = new Rigister();
            fp.Show();



        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;

            if (vm == null || vm.SelectedUser == null)
            {
                MessageBox.Show("لطفاً یک کاربر انتخاب کنید");
                return;
            }

            if (Healpers.LoggedInUser.Role == "Admin" || Healpers.LoggedInUser.Role == "Emploee")
            {
                MessageBox.Show("شما دسترسی حذف کاربر را ندارید");
                return;
            }

            var result = MessageBox.Show( $"آیا از حذف کاربر {vm.SelectedUser.Name} مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            // 4️⃣ حذف از دیتابیس
            using (var db = new AppDbContext())
            {
                var user = db.Employee.Find(vm.SelectedUser.Id);
                if (user != null)
                {
                    db.Employee.Remove(user);
                    db.SaveChanges();
                }
            }

            // 5️⃣ حذف از لیست UI
           // Employee.Remove(vm.SelectedUser.Id);

            MessageBox.Show("کاربر با موفقیت حذف شد");
        }










    }
}
