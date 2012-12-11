using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace interpr {
	public class InputForm : Form {
		private Label label1;
		private TextBox textBox1;
		private Button button1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private Button button2;

		private string m_res;

		public InputForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (InputForm));
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(384, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 40);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(392, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(272, 72);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(144, 72);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(112, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Отмена";
			// 
			// InputForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 101);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputForm";
			this.ShowInTaskbar = false;
			this.Text = "InputForm";
			this.ResumeLayout(false);

		}

		#endregion

		public static bool Input(string caption, string label, out string res) {
			InputForm form = new InputForm();
			form.Text = caption;
			form.label1.Text = label;
			if (form.ShowDialog() == DialogResult.OK) {
				res = form.m_res;
				return true;
			}
			else {
				res = "";
				return false;
			}
		}


		private void button1_Click(object sender, EventArgs e) {
			m_res = textBox1.Text;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == '\r')
				button1_Click(this, new EventArgs());
		}

	}
}