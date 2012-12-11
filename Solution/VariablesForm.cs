using System;
using System.ComponentModel;
using System.Windows.Forms;
using interpr.logic;

namespace interpr {
	public class VariablesForm : Form {
		private ListBox lbVars;
		private Panel panel1;
		private Button button1;
		private Button button2;
		private Button button3;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private ConsoleNamespace.VariableReport[] m_vars; //список переменных
		private Facade m_fasade;

		private VariablesForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public VariablesForm(Facade bridge) : this() {
			m_fasade = bridge;
			m_vars = m_fasade.GetVariables();
			lbVars.Items.Clear();
			foreach (ConsoleNamespace.VariableReport var in m_vars) {
				lbVars.Items.Add(var.name + " : " + var.val);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (VariablesForm));
			this.lbVars = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbVars
			// 
			this.lbVars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbVars.Location = new System.Drawing.Point(0, 0);
			this.lbVars.Name = "lbVars";
			this.lbVars.Size = new System.Drawing.Size(280, 342);
			this.lbVars.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button3);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 309);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(280, 40);
			this.panel1.TabIndex = 1;
			// 
			// button3
			// 
			this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button3.Location = new System.Drawing.Point(184, 8);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(88, 24);
			this.button3.TabIndex = 2;
			this.button3.Text = "Закрыть";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(96, 8);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Удалить все";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "Удалить";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// VariablesForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 349);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lbVars);
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.Name = "VariablesForm";
			this.ShowInTaskbar = false;
			this.Text = "Переменные";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private void button1_Click(object sender, EventArgs e) {
			int index = lbVars.SelectedIndex;
			if (index >= 0) {
				m_fasade.DeleteVariable(m_vars[index].name);
				m_vars = m_fasade.GetVariables();
				lbVars.Items.Clear();
				foreach (ConsoleNamespace.VariableReport var in m_vars) {
					lbVars.Items.Add(var.name + " : " + var.val);
				}
			}
		}

		private void button2_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Вы действительно хотите удалить все переменные?", "",
			                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
			foreach (ConsoleNamespace.VariableReport var in m_vars) {
				m_fasade.DeleteVariable(var.name);
			}
			m_vars = new ConsoleNamespace.VariableReport[] {};
			lbVars.Items.Clear();
		}
	}
}