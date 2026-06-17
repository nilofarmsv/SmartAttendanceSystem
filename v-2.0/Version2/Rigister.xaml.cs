using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WorkSanse.Healpers;
using System.Windows.Navigation;



namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for Rigister.xaml
    /// </summary>
    public partial class Rigister : Window
    {
        
        public Rigister()
        {
            InitializeComponent();
            
        }
      

        public enum PasswordStrength
        {
            Empty = 0,
            VeryWeek = 1,
            Week = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public static class PasswordHelper
        {
            public static PasswordStrength EValuete(string password, out int score)
            {
                score = 0;

                if (string.IsNullOrEmpty(password))
                    return PasswordStrength.Empty;

                if (password.Length >= 8) score += 2;
                else if (password.Length >= 6) score += 1;

                if (Regex.IsMatch(password, "[a-z]")) score += 1;
                if (Regex.IsMatch(password, "[A-Z]")) score += 1;
                if (Regex.IsMatch(password, "[0-9]")) score += 1;
                if (Regex.IsMatch(password, "[^a-zA-Z0-9]")) score += 2;

                if (password.Length < 6) score = Math.Min(score, 1);

                if (score <= 1) return PasswordStrength.VeryWeek;
                if (score <= 2) return PasswordStrength.Week;
                if (score <= 3) return PasswordStrength.Medium;
                if (score <= 5) return PasswordStrength.Strong;
                if (score <= 6) return PasswordStrength.VeryStrong;

                return PasswordStrength.Empty;
            }

            public static (string text, Color color, int progress) StrengthToLabel(PasswordStrength s)
            {
                switch (s)
                {
                    case PasswordStrength.Empty: return ("", Colors.Black, 0);
                    case PasswordStrength.VeryWeek: return ("خیلی ضعیف", Colors.Red, 15);
                    case PasswordStrength.Week: return ("ضعیف", Colors.OrangeRed, 35);
                    case PasswordStrength.Medium: return ("متوسط", Colors.Orange, 60);
                    case PasswordStrength.Strong: return ("قوی", Colors.Green, 80);
                    case PasswordStrength.VeryStrong: return ("خیلی قوی", Colors.DarkGreen, 100);
                    default: return ("", Colors.Black, 0);
                }
            }
        }
        private byte[] uploadedImageBytes;
        private void RegisterUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int score;
                var strength = PasswordHelper.EValuete(Password.Password, out score);

                if (
                    string.IsNullOrWhiteSpace(Name.Text) ||
                    string.IsNullOrWhiteSpace(Family.Text) ||
                    string.IsNullOrWhiteSpace(NationalCode.Text) ||
                    string.IsNullOrWhiteSpace(Password.Text) ||
                    string.IsNullOrWhiteSpace(Phone.Text) ||
                    string.IsNullOrWhiteSpace(RePassword.Text) ||
                    string.IsNullOrWhiteSpace(UserName.Text))
                {
                    CustomMessageBox.Show("لطفا همه‌ی فیلدها را پر کنید");

                    return;
                }

                if (Manegement.IsChecked == false &&
                    Admin.IsChecked == false &&
                    Emploee.IsChecked == false)
                {
                    CustomMessageBox.Show("لطفا سمت را انتخاب کنید");
                    return;
                }

                if (Password.Password != RePassword.Password)
                {
                    CustomMessageBox.Show("رمز عبور با تکرار آن یکسان نیست");

                    return;
                }

                string role = "";

                if (Admin.IsChecked == true)
                    role = Admin.Tag.ToString();
                else if (Manegement.IsChecked == true)
                    role = Manegement.Tag.ToString();
                else if (Emploee.IsChecked == true)
                    role = Emploee.Tag.ToString();
                else
                {
                    CustomMessageBox.Show("لطفاً یک نقش انتخاب کنید");
                    return;
                }




                if (strength == PasswordStrength.VeryWeek || strength == PasswordStrength.Week)
                {
                    CustomMessageBox.Show("رمز عبور ضعیف است. رمز قوی تری انتخاب کنید");
                    return;
                }
                var context = new AppDbContext();
                bool usernameExist = context.User.Any(u => u.UserName == UserName.Text);
                if (usernameExist)
                {
                    CustomMessageBox.Show("این نام کاربری قبلا ثبت شده است.");
                    return;
                }
                var id = Guid.NewGuid();

                var user = new Employee()
                {
                    Id = id,
                    Name = Name.Text,
                    Family = Family.Text,
                    NationalCode = NationalCode.Text,
                    Phone = Phone.Text,
                    HireDate = DateTime.Now,
                    UserName = UserName.Text,
                    PasswordHash = HashHelper.Sha256(Password.Password.Trim()),
                    Position = role,
                    IsActive = true,
                    filePath = uploadedImageBytes
                };

                context.Employee.Add(user);
                context.SaveChanges();


                CustomMessageBox.Show("ثبت‌ نام با موفقیت انجام شد");
                this.Hide();
                Login loginform = new Login();
                loginform.Show();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("خطا در ذخیره‌سازی: " + ex.Message);
            }

        }

        private void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "تصاویر (*.jpg;*.png)|*.jpg;*.png|همه فایل‌ها (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;

                // نمایش عکس در کنترل Image
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                ProfileImage.Source = bitmap; // فرض شده نام Image کنترل ProfileImage است

                // تبدیل عکس به بایت برای ذخیره در دیتابیس یا استفاده دیگر
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        uploadedImageBytes = reader.ReadBytes((int)stream.Length);
                    }
                }
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string pwd = Password.Password;
            int score;

            // بررسی قدرت رمز
            var strength = PasswordHelper.EValuete(pwd, out score);
            var (text, color, progress) = PasswordHelper.StrengthToLabel(strength);



            // نمایش مقدار در ProgressBar
            PasswordStrengthBar.Maximum = 6;
            PasswordStrengthBar.Value = Math.Min(score, 6);

            // تغییر رنگ ProgressBar (در WPF ساده باید با Style یا Foreground تغییر بدهیم)
            switch (strength)
            {
                case PasswordStrength.Empty:
                case PasswordStrength.VeryWeek:
                    PasswordStrengthBar.Foreground = new SolidColorBrush(Colors.Red);
                    break;

                case PasswordStrength.Week:
                    PasswordStrengthBar.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    break;

                case PasswordStrength.Medium:
                    PasswordStrengthBar.Foreground = new SolidColorBrush(Colors.Gold);
                    break;

                case PasswordStrength.Strong:
                    PasswordStrengthBar.Foreground = new SolidColorBrush(Colors.YellowGreen);
                    break;

                case PasswordStrength.VeryStrong:
                    PasswordStrengthBar.Foreground = new SolidColorBrush(Colors.Green);
                    break;
            }

            // فعال/غیرفعال کردن دکمه ثبت نام
            bool okToRegister = (strength == PasswordStrength.Medium || strength == PasswordStrength.Strong || strength == PasswordStrength.VeryStrong);
            PasswordStrengthBar.IsEnabled = okToRegister;

        }
        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
