//namespace Hourglass.GUI.Pages;

//using System.Data.Common;
//using System.Windows.Forms;
//using System.Windows.Forms.Layout;

//public class IPage:TableLayoutPanel {

//	private TableLayoutPanel LeftSidebar;
//	private TableLayoutPanel RightSidebar;
//	private TableLayoutPanel HeaderBar;
//	private TableLayoutPanel BottomBar;
//	private TableLayoutPanel CenterPanel;

//	private bool enable_left_sidebar = true;
//	private bool enable_right_sidebar = true;
//	private bool overlay_left_sidebar = true;
//	private bool overlay_right_sidebar = false;

//	private bool DidCenterPanelSetup = false;

//	private int left_bar_column_weight;
//	private int right_bar_column_weight;
//	private int center_frame_column_weight;

//	private bool ArangementChanged = true;


//	public IPage() {
//		LeftSidebar = new();
//		RightSidebar = new();
//		CenterPanel = new();
//		HeaderBar = new();
//		BottomBar = new();

//		SetVisible();
//		//ApplyLayout();
//	}

//	public IPage(Panel panel) : this() {
//	}

//	public IPage(Form window) : this() {
//	}

//	private void SetVisible() {
//		LeftSidebar.SuspendLayout();
//		RightSidebar.SuspendLayout();
//		HeaderBar.SuspendLayout();
//		BottomBar.SuspendLayout();
//		CenterPanel.SuspendLayout();
//		SuspendLayout();

//		Visible = true;
//		AutoSize = true;
//		ColumnCount = 3;
//		RowCount = 3;
//		BackColor = Color.FromArgb(0, 0, 180);
//		Dock = DockStyle.Fill;
//		Margin = new Padding(0, 0, 0, 0);

//		LeftSidebar.Visible = true;
//		LeftSidebar.AutoSize = true;
//		LeftSidebar.BackColor = Color.FromArgb(255, 0, 0);
//		LeftSidebar.Dock = DockStyle.Fill;
//		LeftSidebar.Margin = new Padding(0, 0, 0, 0);

//		HeaderBar.Visible = true;
//		HeaderBar.AutoSize = true;
//		HeaderBar.BackColor = Color.FromArgb(255, 0, 0);
//		HeaderBar.Dock = DockStyle.Fill;
//		HeaderBar.Margin = new Padding(0, 0, 0, 0);

//		RightSidebar.Visible = true;
//		RightSidebar.AutoSize = true;
//		RightSidebar.BackColor = Color.FromArgb(0, 255, 0);
//		RightSidebar.Dock = DockStyle.Fill;
//		RightSidebar.Margin = new Padding(0,0,0,0);
//		BottomBar.BackColor = Color.FromArgb(0, 100, 200);
//		// 
//		// CenterPanel
//		//
//		CenterPanel.Visible = true;
//		CenterPanel.AutoSize = true;
//		CenterPanel.BackColor = Color.FromArgb(100, 0, 0);
//		CenterPanel.Dock = DockStyle.Fill;
//		CenterPanel.Margin = new Padding(0, 0, 0, 0);

//		//SetupCenterPanel(CenterPanel);

//		LeftSidebar.ResumeLayout(false);
//		RightSidebar.ResumeLayout(false);
//		BottomBar.ResumeLayout(false);
//		CenterPanel.ResumeLayout(false);
//		ResumeLayout(false);
//	}

//	public void ApplyLayout() {
//		if (!DidCenterPanelSetup && CenterPanel != null) {
//			SuspendLayout();
//			CenterPanel.SuspendLayout();
//			SetupCenterPanel(this);
//			CenterPanel.ResumeLayout();
//			ResumeLayout();
//			DidCenterPanelSetup = true;
//		}
		
//		LeftSidebar.SuspendLayout();
//		RightSidebar.SuspendLayout();
//		HeaderBar.SuspendLayout();
//		BottomBar.SuspendLayout();
//		CenterPanel.SuspendLayout();
//		SuspendLayout();

//		Controls.Clear();
//		ColumnStyles.Clear();
//		RowStyles.Clear();

//		ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
//		ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
//		ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
//		RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
//		RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
//		RowStyles.Add(new RowStyle(SizeType.Percent, 10F));

//		Controls.Add(LeftSidebar,9,9);
//		Controls.Add(RightSidebar);
//		Controls.Add(HeaderBar);
//		//Controls.Add(BottomBar);
//		Controls.Add(CenterPanel);

//		if (enable_left_sidebar) {
//			Controls.Add(LeftSidebar, 0, overlay_left_sidebar ? 0 : 1);
//			SetColumnSpan(LeftSidebar, 1);
//			SetRowSpan(LeftSidebar, overlay_left_sidebar ? 2 : 1);
//		}

//		if (enable_right_sidebar) {
//			Controls.Add(RightSidebar, (enable_left_sidebar) ? 2 : 1, overlay_right_sidebar ? 0 : 1);
//			SetColumnSpan(RightSidebar, 1);
//			SetRowSpan(RightSidebar, overlay_right_sidebar ? 2 : 1);
//		}

//		Controls.Add(HeaderBar, (enable_left_sidebar && overlay_left_sidebar) ? 1 : 0, 0);
//		SetColumnSpan(HeaderBar, 1 + ((enable_left_sidebar && overlay_left_sidebar) ? 0 : 1) + ((enable_right_sidebar && overlay_right_sidebar) ? 0 : 1));
//		SetRowSpan(HeaderBar, 1);

//		//Controls.Add(CenterPanel, enable_left_sidebar ? 1 : 0, 1);
//		//SetColumnSpan(CenterPanel, 1 + (enable_left_sidebar ? 0 : 1) + (enable_right_sidebar ? 0 : 1));
//		//SetRowSpan(CenterPanel, 1);

//		ArangementChanged = false;

//		LeftSidebar.ResumeLayout();
//		RightSidebar.ResumeLayout();
//		HeaderBar.ResumeLayout();
//		BottomBar.ResumeLayout();
//		CenterPanel.ResumeLayout();
//		ResumeLayout();
//		PerformLayout();

//	}

//	public virtual void SetupCenterPanel(TableLayoutPanel panel) {
		
//	}
    
//}
