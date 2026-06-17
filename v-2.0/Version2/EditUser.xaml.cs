using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
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
using System.Windows.Shapes;
using WorkSanse.Data;
using WorkSanse.ViewModels;

namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        private EmployeeItemViewModel _employee;
       
        public EditUser(EmployeeItemViewModel employee)
        {
            InitializeComponent();
            _employee = employee;
            FillForm();
        }
        private BitmapImage ByteArrayToImage(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        private void FillForm()
        {
            NameEdit.Text = _employee.Name;
            FamilyEdit.Text = _employee.Family;
            NationalCodeEdit.Text = _employee.NationalCode;
            PhoneEdit.Text = _employee.Phone;
            UserNameEdit.Text = _employee.UserName;


            // سمت
            switch (_employee.Position)
            {
                case "Manegement":
                    ManegementEdit.IsChecked = true;
                    break;
                case "Admin":
                    AdminEdit.IsChecked = true;
                    break;
                case "Emploee":
                    EmploeeEdit.IsChecked = true;
                    break;
            }

            // عکس
            if (_employee.filePath != null && _employee.filePath.Length > 0)
            {
                ProfileImageEdit.Source = ByteArrayToImage(_employee.filePath);
            }

        }

        private void RegisterUser_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Employee.FirstOrDefault(x => x.Id == _employee.Id);

                if (user == null)
                {
                    MessageBox.Show("کاربر در دیتابیس پیدا نشد");
                    return;
                }

                user.Name = NameEdit.Text;
                user.Family = FamilyEdit.Text;
                user.Phone = PhoneEdit.Text;
                user.NationalCode = NationalCodeEdit.Text;
                user.UserName = UserNameEdit.Text;

                if (ManegementEdit.IsChecked == true)
                    user.Position = "Manegement";
                else if (AdminEdit.IsChecked == true)
                    user.Position = "Admin";
                else if (EmploeeEdit.IsChecked == true)
                    user.Position = "Emploee";

                if (_employee.filePath != null && _employee.filePath.Length > 0)
                {
                    user.filePath = _employee.filePath;
                }

                db.SaveChanges();
            }

            MessageBox.Show("ویرایش با موفقیت انجام شد");
            this.Close();
           
        }
        private void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png"
            };

            if (openFile.ShowDialog() == true)
            {
                byte[] imageBytes = File.ReadAllBytes(openFile.FileName);

                ProfileImageEdit.Source = ByteArrayToImage(imageBytes);

                // ذخیره موقت برای ویرایش
                _employee.filePath = imageBytes;
            }
        }
        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
