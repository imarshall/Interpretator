using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using interpr.logic;

namespace interpr {
	public class EditorForm : Form {
		private SourceBox sbEdit;
		private Panel panel1;
		private Button button1;
		private Button button2;
		private IContainer components;

		private static ArrayList s_opened_functions = new ArrayList();

		private Facade m_fasade;
		private FileInfo m_file;
		private Timer timer1;
		private Label label1;
		private string m_name;


		public EditorForm() {
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

		public static bool IsOpened(string name) {
			return s_opened_functions.IndexOf(name) >= 0;
		}

		public static bool ThereAreOpened() {
			return s_opened_functions.Count > 0;
		}

		public static EditorForm EditFunction(string name, Facade bridge) {
			if (s_opened_functions.IndexOf(name) >= 0)
				return null;
			if (!Parser.IsUserID(name)) {
				MessageBox.Show("Неправильное имя функции");
				return null;
			}
			if (IsOpened(name)) {
				MessageBox.Show("Функция уже открыта");
				return null;
			}
			EditorForm ef = new EditorForm();
			ef.m_fasade = bridge;
			ef.m_file = new FileInfo(Directory.GetCurrentDirectory() + @"\subroutines\" + name);
			ef.sbEdit.LoadFile(ef.m_file.FullName);
			ef.m_name = name;
			ef.Text = name;
			s_opened_functions.Add(name);
			return ef;
		}

		private bool Save() {
			lock (this) {
				if (m_fasade.Busy) {
					MessageBox.Show("Не могу сохранить функцию во время выполнения команды");
					return false;
				}
				if (!sbEdit.SaveFile(m_file.FullName))
					return false;
				if (!m_fasade.LoadSub(m_name)) {
					MessageBox.Show("Функция сохранена, но она содержит синтаксическую ошибку\n");
					m_fasade.UnloadSub(m_name);
				}
				this.Activate();
				return true;
			}
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (EditorForm));
			this.sbEdit = new SourceBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// sbEdit
			// 
			this.sbEdit.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.sbEdit.Location = new System.Drawing.Point(8, 32);
			this.sbEdit.Name = "sbEdit";
			this.sbEdit.Size = new System.Drawing.Size(720, 416);
			this.sbEdit.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(736, 32);
			this.panel1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(232, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 23);
			this.label1.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(112, 0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(112, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Выход";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(0, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(112, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "Сохранить";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// EditorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(736, 453);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.sbEdit);
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.Name = "EditorForm";
			this.Text = "Editor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.EditorForm_Closing);
			this.Closed += new System.EventHandler(this.EditorForm_Closed);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private void EditorForm_Closing(object sender, CancelEventArgs e) {
			DialogResult dr = MessageBox.Show("", "Сохранить ?", MessageBoxButtons.YesNoCancel);
			if (dr == DialogResult.Yes) {
				if (Save())
					e.Cancel = false;
				else {
					MessageBox.Show("Ошибка при сохранении");
					e.Cancel = true;
				}
			}
			else if (dr == DialogResult.No)
				e.Cancel = false;
			else if (dr == DialogResult.Cancel)
				e.Cancel = true;
		}

		private void button1_Click(object sender, EventArgs e) {
			Save();
		}

		private void button2_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void EditorForm_Closed(object sender, EventArgs e) {
			s_opened_functions.Remove(m_name);
		}

		private void timer1_Tick(object sender, EventArgs e) {
			label1.Text = "Строка " + (sbEdit.CurrentLine + 1).ToString();
		}
	}
}