namespace Hourglass.GUI;

using Hourglass.GUI.Pages;

using System.Windows.Forms;

public partial class MainWindow : Form {
	public List<IPage> PageBuffer;
	private IPage? CurrentPage;

	public MainWindow() {
		PageBuffer = new();
		components = new System.ComponentModel.Container();
		//InitializeComponent();
		ShowPage<TimerPage>();
		//ShowPage<TimerPage>();
		//Resize += OnResize;
	}

	public void ShowPage<T>() where T : IPage {
		SuspendLayout();
		if (components == null)
			return;
		foreach (object p in Controls) {
			if (p.GetType().Equals(CurrentPage?.GetType()))
				Controls.Remove((IPage)p);
		}
		bool exists = false;

		foreach (IPage p in PageBuffer) {
			if (p.GetType().Equals(typeof(T))) {
				exists = true;
			}
		}

		if (!exists) {
			PageBuffer.Add((T)Activator.CreateInstance(typeof(T)));
		}

		foreach (IPage p in PageBuffer) {
			if (p.GetType().Equals(typeof(T))) {
				this.Controls.Add(p);
				p.Size = Size;
				CurrentPage = p;
			}
		}
		ResumeLayout();
	}
}