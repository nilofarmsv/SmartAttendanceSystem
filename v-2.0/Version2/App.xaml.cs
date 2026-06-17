using OfficeOpenXml;
using QuestPDF.Infrastructure;
using System.Configuration;
using System.Data;
using System.Windows;

namespace WorkSanse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ExcelPackage.License.SetNonCommercialPersonal("ZahraSadatMosaviNiko");
            base.OnStartup(e);
            QuestPDF.Settings.License = LicenseType.Community;
            base.OnStartup(e);
        }
    }

}
