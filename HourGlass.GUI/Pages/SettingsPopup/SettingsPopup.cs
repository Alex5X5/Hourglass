using Hourglass.Util.Services;

namespace Hourglass.GUI.Pages.SettingsPopup {
    public partial class SettingsPopup : Form {

        public SettingsPopup() {
            InitializeComponent();
            //JobTetbox.Text = SettingsService.GetSetting(SettingsService.JOB_NAME_KEY);
            //NameTextbox.Text = SettingsService.GetSetting(SettingsService.USER_NAME_KEY);
            //StartDateTextbox.Text = SettingsService.GetSetting(SettingsService.START_DATE_KEY);
        }

        private void ParseEnteredValues() {
            //SettingsService.SetSetting(SettingsService.JOB_NAME_KEY, JobTetbox.Text);
            //SettingsService.SetSetting(SettingsService.USER_NAME_KEY, NameTextbox.Text);
            //SettingsService.SetSetting(SettingsService.START_DATE_KEY, StartDateTextbox.Text);
        }

        private void OkButton_Click(object sender, EventArgs e) {
            ParseEnteredValues();
            //SettingsService.ReloadSettings();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void SettingsPopup_Load(object sender, EventArgs e) {
            webBrowser1.Navigate(@"C:\Users\lehnerale\source\repos\Hourglass\HourGlass\bin\Debug\net9.0-windows\win-x64\Hourglass\Nachweise\Ausbildungsnachweis71_22.9. 2025-26.9. 2025.pdf");
        }
    }
}
