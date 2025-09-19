using Hourglass.Util.Services;

namespace Hourglass.GUI.Pages.SettingsPopup {
	public partial class SettingsPopup : Form {
		
		private readonly Dictionary<string, string> Settings;

		public const string NAME_KEY = "name";
		public const string JOB_KEY = "job";
		
		public SettingsPopup() {
			InitializeComponent();
			Settings = SettingsService.ReadAppSettings();
			Settings.TryGetValue(JOB_KEY, out string? job);
			JobTetbox.Text = job ?? "";
			Settings.TryGetValue(NAME_KEY, out string? name);
			NameTextbox.Text = name ?? "";
		}

		private void ParseEnteredValues() {
			Settings[JOB_KEY] = JobTetbox.Text;
			Settings[NAME_KEY] = NameTextbox.Text;
		}

		private void OkButton_Click(object sender, EventArgs e) {
			ParseEnteredValues();
			string[] lines = new string[Settings.Count];
			int i = 0;
			foreach (string key in Settings.Keys) {
				lines[i] = key + ":" + Settings[key];
				i++;
			}
			string res = string.Join("\n", lines);
			string path = PathService.FilesPath("appsettings.dat");
            if (!File.Exists(path))
				File.Create(path);
			using FileStream fileHandle = File.Open(path, FileMode.Truncate);
			using StreamWriter streamWriter = new(fileHandle);
			streamWriter.Write(res);
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
