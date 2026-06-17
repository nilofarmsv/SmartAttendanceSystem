using Microsoft.Win32;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkSanse.Data;
using WorkSanse.Healpers;
using WorkSanse.Services;
using WorkSanse.Services.WorkSanse.Services;



namespace WorkSanse.ViewModels
{
    public class ManageAttendanceViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<AttendanceViewModel> Attendances { get; set; }
        public ObservableCollection<AttendanceViewModel> MyAttendances { get; set; }

        private AttendanceViewModel _selectedAttendance;
        public AttendanceViewModel SelectedAttendance
        {
            get => _selectedAttendance;
            set
            {
                _selectedAttendance = value;
                OnPropertyChanged();
            }
        }

        // Commands

        public ICommand DeleteCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand ExportPdfCommand { get; set; }
        public ICommand CheckInCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public ICommand IsFingerCommand { get; set; }

        public ManageAttendanceViewModel()
        {
            Attendances = new ObservableCollection<AttendanceViewModel>();

            MyAttendances = new ObservableCollection<AttendanceViewModel>();

            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedAttendance != null);
            ExportExcelCommand = new RelayCommand(_ => ExportExcel());
            ExportPdfCommand = new RelayCommand(_ => ExportPdf());
            CheckInCommand = new RelayCommand(_ => CheckIn());
            CheckOutCommand = new RelayCommand(_ => CheckOut());
            LoadMyAttendances();
            LoadFromDb();
        }

        private async void CheckIn()
        {
            using var db = new AppDbContext();
            var userId = LoggedInUser.UserId;

            var attendance = new Data.Attendance
            {
                EmployeeId = userId,
                Date = DateTime.Today,
                CheckInTime = DateTime.Now
            };

            db.Attendance.Add(attendance);
            db.SaveChanges();
            var telegramUser = db.TelegramUsers.FirstOrDefault(t => t.EmploeeId == userId && t.IsVerified);

            if (telegramUser != null)
            {
                var telegram = new TelegramService();
                //await telegram.SendMessageAsync(
                //    telegramUser.ChatId,
                //    "✅ ورود شما با موفقیت ثبت شد"
                //);
            }
            LoadMyAttendances(); // دیتاگرید آپدیت میشه
        }

        private void CheckOut()
        {
            using var db = new AppDbContext();
            var userId = LoggedInUser.UserId;

            var attendance = db.Attendance
                               .Where(a => a.EmployeeId == userId && a.Date == DateTime.Today)
                               .FirstOrDefault();

            if (attendance != null)
            {
                attendance.CheckOutTime = DateTime.Now;
                db.SaveChanges();
                LoadMyAttendances(); // دیتاگرید آپدیت میشه
            }
        }
        private void LoadMyAttendances()
        {
            using var db = new AppDbContext();
            var userId = Healpers.LoggedInUser.UserId;

            var data = db.Attendance
                         .Where(a => a.EmployeeId == userId)
                         .OrderByDescending(a => a.Date)
                         .ToList();

            MyAttendances.Clear();
            foreach (var a in data)
            {
                MyAttendances.Add(new AttendanceViewModel
                {
                    Date = a.Date,
                    CheckIn = a.CheckInTime.HasValue ? a.CheckInTime.Value.ToString("HH:mm") : "-",
                    CheckOut = a.CheckOutTime.HasValue ? a.CheckOutTime.Value.ToString("HH:mm") : "-",
                    Status = a.CheckOutTime == null ? "حاضر" : "خارج شده"
                });
            }
        }
        private void LoadFromDb()
        {
            try
            {


                Attendances.Clear();

                using var db = new AppDbContext();

                var data = db.Attendance
                    .Join(db.Employee,
                          a => a.EmployeeId,
                          e => e.Id,
                          (a, e) => new AttendanceViewModel
                          {
                              Id = a.Id,
                              Name = e.Name,
                              CheckIn = a.CheckInTime.HasValue ? a.CheckInTime.Value.ToString("HH:mm") : "-",
                              CheckOut = a.CheckOutTime.HasValue ? a.CheckOutTime.Value.ToString("HH:mm") : "-",
                              Status = a.CheckOutTime == null ? "حاضر" : "خارج شده"
                          })
                    .OrderByDescending(x => x.Name)
                    .ToList();

                foreach (var item in data)
                    Attendances.Add(item);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Delete()
        {
            if (SelectedAttendance == null) return;

            using var db = new AppDbContext();

            var entity = db.Attendance.FirstOrDefault(x => x.Id == SelectedAttendance.Id);
            if (entity == null) return;

            db.Attendance.Remove(entity);
            db.SaveChanges();

            Attendances.Remove(SelectedAttendance);
        }

        private void ExportExcel()
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "Excel File (*.xlsx)|*.xlsx",
                FileName = "Attendance.xlsx"
            };

            if (saveDialog.ShowDialog() != true)
                return;

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Attendance");

            // هدرها
            sheet.Cells[1, 1].Value = "نام";
            sheet.Cells[1, 2].Value = "ورود";
            sheet.Cells[1, 3].Value = "خروج";
            sheet.Cells[1, 4].Value = "وضعیت";

            int row = 2;
            foreach (var a in Attendances)
            {
                sheet.Cells[row, 1].Value = a.Name;
                sheet.Cells[row, 2].Value = a.CheckIn;
                sheet.Cells[row, 3].Value = a.CheckOut;
                sheet.Cells[row, 4].Value = a.Status;
                row++;
            }

            File.WriteAllBytes(saveDialog.FileName, package.GetAsByteArray());
        }
       
        private void ExportPdf()
        {

            var saveDialog = new SaveFileDialog
            {
                Filter = "PDF File (*.pdf)|*.pdf",
                FileName = "Attendance.pdf"
            };

            if (saveDialog.ShowDialog() != true)
                return;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x
                        .FontFamily("Vazirmatn")
                        .FontSize(11)
                    );

                    page.Content()
                        .Column(column =>
                        {
                            // عنوان
                            column.Item()
                                .AlignRight()
                                .Text("گزارش حضور و غیاب")
                                .FontSize(18)
                                .Bold();

                            column.Item().PaddingVertical(10);

                            // جدول
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                // هدر
                                table.Header(header =>
                                {
                                    header.Cell().Border(1).Padding(5).Text("نام").Bold();
                                    header.Cell().Border(1).Padding(5).Text("ورود").Bold();
                                    header.Cell().Border(1).Padding(5).Text("خروج").Bold();
                                    header.Cell().Border(1).Padding(5).Text("وضعیت").Bold();
                                });

                                foreach (var a in Attendances)
                                {
                                    table.Cell().Border(1).Padding(5).Text(a.Name);
                                    table.Cell().Border(1).Padding(5).Text(a.CheckIn);
                                    table.Cell().Border(1).Padding(5).Text(a.CheckOut);
                                    table.Cell().Border(1).Padding(5).Text(a.Status);
                                }
                            });
                        });
                });
            })
            .GeneratePdf(saveDialog.FileName);

            MessageBox.Show("PDF با موفقیت ساخته شد ✅");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}


