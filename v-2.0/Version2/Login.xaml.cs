using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using WorkSanse.Data;
using Wpf.Ui.Controls;


namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            if (Properties.Settings.Default.RemmeberMe)
            {
                UsernameTextBox.Text = Properties.Settings.Default.Username;
                RememberMeCheckBox.IsChecked = true;
            }

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Text))
                {
                    CustomMessageBox.Show("لطفا همه‌ی فیلدها را پر کنید");
                    return;
                }

                using (var context = new AppDbContext())
                {
                    var username = UsernameTextBox.Text.Trim();
                    var password = PasswordBox.Password.Trim();
                    var hashedPassword = Healpers.HashHelper.Sha256(password);

                    var user = context.Employee
                        .Where(u => u.UserName == username && u.PasswordHash == hashedPassword).Select(u => new { u.Id, u.UserName, u.PasswordHash, u.Name, u.Family, u.Position,u.filePath }).FirstOrDefault();

                    if (user != null)
                    {
                        if (RememberMeCheckBox.IsChecked == true)
                        {
                            Properties.Settings.Default.Username = username;
                            Properties.Settings.Default.RemmeberMe = true;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default.Username = "";

                            Properties.Settings.Default.RemmeberMe = false;
                            Properties.Settings.Default.Save();
                        }
                        CustomMessageBox.Show($"خوش آمدید {user.Name} {user.Family}");


                        var fullName = $"{user.Name} {user.Family}";


                        if (user.Position == "Admin" || user.Position == "Manegement")
                        {
                            Healpers.LoggedInUser.UserId = user.Id;
                            Healpers.LoggedInUser.Username = user.Name;
                            Healpers.LoggedInUser.Role = user.Position;
                            Healpers.LoggedInUser.Image = user.filePath;

                            MainWindow userPanel = new MainWindow(fullName);
                            userPanel.Show();
                            this.Hide();
                        }
                        else
                        {
                            Healpers.LoggedInUser.UserId = user.Id;
                            Healpers.LoggedInUser.Username = user.Name;
                            Healpers.LoggedInUser.Role = user.Position;
                            Healpers.LoggedInUser.Image = user.filePath;
                            UserWindow formAdmin = new UserWindow(fullName);
                            formAdmin.Show();
                            this.Hide();
                            return;
                        }

                    }
                    else
                    {
                        CustomMessageBox.Show("نام کاربری یا رمز عبور اشتباه است");
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            Rigister fp = new Rigister();
            fp.Show();

            // اگر میخوای صفحه فعلی بسته بشه:
            this.Close();
        }



    }
}
