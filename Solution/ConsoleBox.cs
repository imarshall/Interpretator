using System;
using System.ComponentModel;
using System.Windows.Forms;
using interpr.logic;

namespace interpr {
	[DefaultEvent("GetCommand")]
	public class ConsoleBox : UserControl, IConsole {
		private RichTextBox rtb;

		private Container components = null;

		private bool m_waiting = false; 
		private int m_inputbeg; 
		private static string s_str;

		public ConsoleBox() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.rtb = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// rtb
			// 
			this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtb.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (204)));
			this.rtb.Location = new System.Drawing.Point(0, 0);
			this.rtb.Name = "rtb";
			this.rtb.Size = new System.Drawing.Size(440, 248);
			this.rtb.TabIndex = 0;
			this.rtb.Text = "";
			this.rtb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtb_KeyPress);
			this.rtb.Protected += new System.EventHandler(this.rtb_Protected);
			// 
			// ConsoleBox
			// 
			this.Controls.Add(this.rtb);
			this.Name = "ConsoleBox";
			this.Size = new System.Drawing.Size(440, 248);
			this.ResumeLayout(false);

		}

		#endregion

		public event ConsoleBoxGetCommandEventHandler GetCommand;

		private void AddText() {
			rtb.Text = rtb.Text + s_str + '\n';
			if (m_waiting) {
				lock (this) {
					m_waiting = false;
					Prompt();
				}
			}
		}

		public void Prompt() {
			(new MethodInvoker(DoPrompt)).Invoke();
		}

		private void DoPrompt() {
			lock (this) {
				s_str = ">>>";
				AddText();
				int oldinputbeg = m_inputbeg;
				m_inputbeg = rtb.TextLength;
				rtb.SelectionStart = oldinputbeg;
				rtb.SelectionLength = m_inputbeg - oldinputbeg;
				rtb.SelectionProtected = true;
				rtb.SelectionStart = rtb.TextLength;
				rtb.SelectionLength = 0;
				rtb.SelectionProtected = false;
				m_waiting = true;
				this.Focus();
			}
		}

		public void Print(string str) {
			lock (this) {
				s_str = str;
				(new MethodInvoker(AddText)).Invoke();
			}
		}

		public void PrintLn(string str) {
			lock (this) {
				s_str = str + "\r\n";
				(new MethodInvoker(AddText)).Invoke();
			}
		}

		private void rtb_KeyPress(object sender, KeyPressEventArgs e) {
			if (m_waiting && e.KeyChar == '\r') {
				string cmd = rtb.Text.Substring(m_inputbeg);
				m_waiting = false;
				GetCommand(this,
				           new ConsoleBoxGetCommandEventArgs(cmd));
			}
		}

		private void rtb_Protected(object sender, EventArgs e) {
			rtb.SelectionStart = rtb.TextLength;
			rtb.SelectionLength = 0;
			rtb.SelectionProtected = false;
		}

	}

	public delegate void ConsoleBoxGetCommandEventHandler(object sender,
	                                                      ConsoleBoxGetCommandEventArgs e);

	public class ConsoleBoxGetCommandEventArgs : EventArgs {
		private string m_command;

		public ConsoleBoxGetCommandEventArgs(string command) {
			m_command = command;
		}

		public string Command {
			get { return m_command.Trim(); }
		}
	}


}