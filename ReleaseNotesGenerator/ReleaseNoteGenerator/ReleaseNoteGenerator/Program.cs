using System;
using System.Security.Authentication;
using System.Windows.Forms;
using V1ServicesConnector.Classes;
using V1ServicesConnector.Form;
using ReleaseNoteController;
using ReleaseNotesWriter.Writer.Concrete;

namespace ReleaseNoteGenerator {
    static class Program {
        private static V1Services _mV1Services;
        static void SetServicesInstance() {
            _mV1Services = V1Services.Instance;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length == 0) {
                return RunGui();
            }
            else if (args.Length > 3) {
                return RunCmd(args);
            }
            else return 1;
        }
        private static int RunGui()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetServicesInstance();
            try {
                LogInForm LogIn = new LogInForm(true);
                if (_mV1Services.IsServicesConnected()) {
                    ExcelWriter excel = new ExcelWriter(_mV1Services);
                    Application.Run(new Menu());
                }
            }
            catch (InvalidCredentialException e) {
                Console.WriteLine(e.Message);
            }
            return 0;
        }
        private static int RunCmd(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetServicesInstance();
            try
            {
                LogInForm LogIn = new LogInForm(true);
                if (_mV1Services.IsServicesConnected()) {
                    ExcelWriter excel = new ExcelWriter(_mV1Services);
                    CmdRun commandLineExecutor = new CmdRun();
                    return Convert.ToInt32(commandLineExecutor.Run(args));
                }
            }
            catch (InvalidCredentialException e) {
                Console.WriteLine(e.Message);
            }
            return 1;
        }
    }
}
