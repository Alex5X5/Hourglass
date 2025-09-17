using Hourglass.Util.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hourglass.GUI.Pages.SettingsPopup {
	public partial class SettingsPopup : Form {
		public SettingsPopup() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			string[] buffer = ["", "", "", ""];
			string[] lines;
			using (FileStream fileHandle = File.Open(PathService.AssetsPath("appsettings.dat"), FileMode.OpenOrCreate))
			using (StreamReader streamReader = new StreamReader(fileHandle))
				lines = streamReader.ReadToEnd().Split('\n');
		}

		private void button2_Click(object sender, EventArgs e) {
			
		}
	}
}
