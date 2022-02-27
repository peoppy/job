using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraEditors;

namespace Sklad.Win {
    static class Program {

        [STAThread]
        static void Main() {
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.Latest;
            WindowsFormsSettings.LoadApplicationSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			DevExpress.Utils.ToolTipController.DefaultController.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            if(Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder) {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            SkladWindowsFormsApplication winApplication = new SkladWindowsFormsApplication();
            winApplication.GetSecurityStrategy().RegisterXPOAdapterProviders();

            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }

            if(System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }

            try {
                winApplication.Setup();
                winApplication.Start();
            }
            catch(Exception e) {
                winApplication.StopSplash();
                winApplication.HandleException(e);
            }
        }
    }
}
