using Hourglass.Util.Services;

namespace Hourglass.GUI.Pages.LoginPopup {
	public partial class LoginPopup : Form {

		public LoginPopup() {
			InitializeComponent();
		}

		private void LoginPopup_Load(object sender, EventArgs e) {

		}

		private void LoginButton_Click(object sender, EventArgs e) {
			EncryptionService service = new(textBox1.Text);
			service.DecryptFile(PathService.FilesPath("database"));
		}
	}
}
