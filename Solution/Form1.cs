using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace interpr {
	public class Form1 : Form {
		private Panel panel1;
		private Button button1;
		private Button button2;
		private Button button3;
		private Button button4;
		private Button button5;
		private ConsoleBox consoleBox1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public Form1() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (Form1));
			this.panel1 = new System.Windows.Forms.Panel();
			this.button5 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.consoleBox1 = new interpr.ConsoleBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button5);
			this.panel1.Controls.Add(this.button4);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(624, 24);
			this.panel1.TabIndex = 1;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(360, 0);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(144, 24);
			this.button5.TabIndex = 3;
			this.button5.Text = "Сохранить переменные";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(504, 0);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(120, 24);
			this.button4.TabIndex = 1;
			this.button4.Text = "Выход";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(120, 0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(120, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Переменные";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(0, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "Функции";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(240, 0);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(120, 24);
			this.button3.TabIndex = 2;
			this.button3.Text = "Перезапуск";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// consoleBox1
			// 
			this.consoleBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.consoleBox1.Location = new System.Drawing.Point(0, 24);
			this.consoleBox1.Name = "consoleBox1";
			this.consoleBox1.Size = new System.Drawing.Size(624, 397);
			this.consoleBox1.TabIndex = 0;
			this.consoleBox1.GetCommand += new interpr.ConsoleBoxGetCommandEventHandler(this.consoleBox1_GetCommand);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(624, 421);
			this.Controls.Add(this.consoleBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Интерактивный интерпретатор";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main() {
			Application.Run(new Form1());
		}

		private Facade m_fasade;

		private void Form1_Load(object sender, EventArgs e) {
			Facade.Create(consoleBox1);
			m_fasade = Facade.Instance;
			if (m_fasade.NotRestored) {
				MessageBox.Show("Ошибка! Переменные не были успешно восстановлены.");
			}
			m_fasade.Done += new Facade.CommandDoneHandler(EndExec);
			m_fasade.LoadSubs();
			consoleBox1.Prompt();
		}

		private void EndExec() {
			consoleBox1.Prompt();
		}

		private void button1_Click(object sender, EventArgs e) {
			if (m_fasade.Busy) {
				MessageBox.Show("Не могу открыть окно функций во время выполнения комманды!");
				return;
			}
			FunctionsForm ff = new FunctionsForm(m_fasade);
			ff.ShowDialog();
			EditorForm ef = ff.LastOpenedEditorForm;
			if (ef != null) {
				ef.Activate();
				ff.SetLastEditorFormNull();
			}
			else
				consoleBox1.Focus();
		}

		private void button2_Click(object sender, EventArgs e) {
			if (m_fasade.Busy) {
				MessageBox.Show("Не могу открыть окно переменных во время выполнения комманды!");
				return;
			}
			VariablesForm vf = new VariablesForm(m_fasade);
			vf.ShowDialog();
			consoleBox1.Focus();
		}

		private void consoleBox1_GetCommand(object sender, ConsoleBoxGetCommandEventArgs e) {
			if (e.Command.Length > 0)
				m_fasade.ExecuteCommand(e.Command);
			else
				consoleBox1.Prompt();
		}

		private void button3_Click(object sender, EventArgs e) {
			m_fasade.Restart();
			if (m_fasade.NotRestored) {
				MessageBox.Show("Ошибка! Переменные не были успешно восстановлены.");
			}
			consoleBox1.Focus();
		}

		private void button5_Click(object sender, EventArgs e) {
			if (m_fasade.Busy) {
				MessageBox.Show("Не могу сохранить переменные во время выполнения программы");
				return;
			}
			m_fasade.SaveVariables();
			consoleBox1.Focus();
		}

		private void Form1_Closing(object sender, CancelEventArgs e) {
			if (EditorForm.ThereAreOpened()) {
				MessageBox.Show("Сначала закройте все окна редактора кода.");
				e.Cancel = true;
				return;
			}
			m_fasade.SaveVariables();
		}

		private void button4_Click(object sender, EventArgs e) {
			this.Close();
		}


	}
}