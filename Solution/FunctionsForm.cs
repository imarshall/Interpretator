using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace interpr {
	public class FunctionsForm : Form {
		private ListBox listBox1;
		private Button button1;
		private Button button2;
		private Button button3;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private Facade m_fasade;
		private Panel panel1;
		private Button button4;
		private Label label1;
		private ListBox listBox2;
		private Button button5;
		private Button button6;
		private string[] m_functions;
		private EditorForm m_last_ef = null;

		public EditorForm LastOpenedEditorForm {
			get { return m_last_ef; }
		}

		public void SetLastEditorFormNull() {
			m_last_ef = null;
		}

		public FunctionsForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public FunctionsForm(Facade facade) : this() {
			m_fasade = facade;
			listBox1.Items.Clear();
			m_functions = m_fasade.GetSubs();
			foreach (string fun in m_functions) {
				listBox1.Items.Add(fun);
			}
			DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\subroutines");
			FileInfo[] files = di.GetFiles();
			listBox2.Items.Clear();
			foreach (FileInfo file in files) {
				if (Array.IndexOf(m_functions, file.Name) < 0)
					listBox2.Items.Add(file.Name);
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (FunctionsForm));
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button4 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 48);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(448, 238);
			this.listBox1.TabIndex = 0;
			this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(112, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(96, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "Редактировать";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(8, 8);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(96, 24);
			this.button2.TabIndex = 2;
			this.button2.Text = "Создать";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(216, 8);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(104, 24);
			this.button3.TabIndex = 3;
			this.button3.Text = "Удалить";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button4);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.button3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(464, 40);
			this.panel1.TabIndex = 4;
			// 
			// button4
			// 
			this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button4.Location = new System.Drawing.Point(344, 8);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(104, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "Закрыть";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 296);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Функции с ошибками:";
			// 
			// listBox2
			// 
			this.listBox2.Location = new System.Drawing.Point(8, 360);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(448, 134);
			this.listBox2.TabIndex = 6;
			this.listBox2.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(16, 320);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(104, 23);
			this.button5.TabIndex = 7;
			this.button5.Text = "Редактировать";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(136, 320);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(104, 23);
			this.button6.TabIndex = 8;
			this.button6.Text = "Удалить";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// FunctionsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 503);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.listBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FunctionsForm";
			this.Text = "Функции";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private void button2_Click(object sender, EventArgs e) {
			string name;
			StreamWriter sw = null;
			if (InputForm.Input("", "Введите имя функции", out name)) {
				if (name == "") {
					MessageBox.Show("Неправильное имя функции");
					return;
				}
				try {
					sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\subroutines\" + name);
					sw.WriteLine(name + " []");
				}
				finally {
					if (sw != null)
						sw.Close();
				}
				EditorForm ef = EditorForm.EditFunction(name, m_fasade);
				if (ef == null) {
					MessageBox.Show("Ошибка при создании функции");
					FileInfo fi = new FileInfo(Directory.GetCurrentDirectory() + @"\subroutines\" + name);
					if (fi.Exists) {
						fi.Delete();
					}
				}
				else {
					m_fasade.LoadSub(name);
					this.Close();
					ef.Show();
					m_last_ef = ef;
				}
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			if (listBox1.SelectedIndex < 0)
				return;
			EditorForm ef = EditorForm.EditFunction(listBox1.SelectedItem.ToString(), m_fasade);
			if (ef == null)
				MessageBox.Show("Ошибка при открытии функции");
			else {
				this.Close();
				ef.Show();
				m_last_ef = ef;
			}
		}

		private void button3_Click(object sender, EventArgs e) {
			if (listBox1.SelectedIndex < 0)
				return;
			string name = listBox1.SelectedItem.ToString();
			if (EditorForm.IsOpened(name)) {
				MessageBox.Show("Не могу удалить открытую в редакторе функцию");
				return;
			}
			if (MessageBox.Show("Вы действительно хотите удалить функцию " + name, "",
			                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
				return;
			try {
				File.Delete(Directory.GetCurrentDirectory() + @"\subroutines\" + name);
				listBox1.Items.Remove(name);
				m_fasade.UnloadSub(name);
			}
			catch {
				MessageBox.Show("Ошибка при удалении функции");
			}
		}

		private void listBox1_DoubleClick(object sender, EventArgs e) {
			if (listBox1.SelectedIndex < 0)
				return;
			EditorForm ef = EditorForm.EditFunction(listBox1.SelectedItem.ToString(), m_fasade);
			if (ef == null)
				MessageBox.Show("Ошибка при открытии функции");
			else {
				this.Close();
				ef.Show();
				m_last_ef = ef;
			}
		}

		private void button5_Click(object sender, EventArgs e) {
			if (listBox2.SelectedIndex < 0)
				return;
			EditorForm ef = EditorForm.EditFunction(listBox2.SelectedItem.ToString(), m_fasade);
			if (ef == null)
				MessageBox.Show("Ошибка при открытии функции");
			else {
				this.Close();
				ef.Show();
				m_last_ef = ef;
			}
		}

		private void listBox2_DoubleClick(object sender, EventArgs e) {
			if (listBox2.SelectedIndex < 0)
				return;
			EditorForm ef = EditorForm.EditFunction(listBox2.SelectedItem.ToString(), m_fasade);
			if (ef == null)
				MessageBox.Show("Ошибка при открытии функции");
			else {
				this.Close();
				ef.Show();
				m_last_ef = ef;
			}
		}

		private void button6_Click(object sender, EventArgs e) {
			if (listBox2.SelectedIndex < 0)
				return;
			string name = listBox2.SelectedItem.ToString();
			if (EditorForm.IsOpened(name)) {
				MessageBox.Show("Не могу удалить открытую в редакторе функцию");
				return;
			}
			if (MessageBox.Show("Вы действительно хотите удалить функцию " + name, "",
			                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
				return;
			try {
				File.Delete(Directory.GetCurrentDirectory() + @"\subroutines\" + name);
				listBox2.Items.Remove(name);
			}
			catch {
				MessageBox.Show("Ошибка при удалении функции");
			}
		}

	}
}