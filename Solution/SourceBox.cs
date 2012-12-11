using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace interpr {
	public class SourceBox : UserControl {
		private RichTextBox m_tb; 
		private TextBox m_tb_2; 

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public SourceBox() {
			// This call is required by the Windows.Forms Form Designer.
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.m_tb = new System.Windows.Forms.RichTextBox();
			this.m_tb_2 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// m_tb
			// 
			this.m_tb.AcceptsTab = true;
			this.m_tb.BackColor = System.Drawing.Color.White;
			this.m_tb.DetectUrls = false;
			this.m_tb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tb.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (204)));
			this.m_tb.Location = new System.Drawing.Point(0, 0);
			this.m_tb.Name = "m_tb";
			this.m_tb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.m_tb.ShowSelectionMargin = true;
			this.m_tb.Size = new System.Drawing.Size(150, 150);
			this.m_tb.TabIndex = 0;
			this.m_tb.Text = "";
			this.m_tb.WordWrap = false;
			this.m_tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_tb_KeyPress);
			this.m_tb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_tb_MouseUp);
			this.m_tb.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_tb_KeyUp);
			// 
			// m_tb_2
			// 
			this.m_tb_2.Location = new System.Drawing.Point(-80, -16);
			this.m_tb_2.Name = "m_tb_2";
			this.m_tb_2.TabIndex = 1;
			this.m_tb_2.Text = "textBox1";
			// 
			// SourceBox
			// 
			this.Controls.Add(this.m_tb);
			this.Controls.Add(this.m_tb_2);
			this.Name = "SourceBox";
			this.ResumeLayout(false);

		}

		#endregion

		private int m_curline = 0;//текущая строка
		private int m_lincount = 0;//общее число строк

		private HighlightParser m_hp = new HighlightParser();

		private static Font s_nfont =
			new Font("Lucida Console", 10, FontStyle.Regular);

		private static Font s_cfont =
			new Font("Lucida Console", 12, FontStyle.Bold);

		private int GetCurrentLine() {
			return m_tb.GetLineFromCharIndex(m_tb.SelectionStart);
		}

		private int GetLinesCount() {
			return m_tb.Lines.Length;
		}

		private String GetLine(int index) {
			return m_tb.Lines[index];
		}

		private void m_tb_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == '\r') {
				string txt = m_tb.Text;
				int i = m_tb.SelectionStart - 2;
				int j;
				while (i >= 0) {
					if (txt[i] == '\n')
						return;
					else if (txt[i] == '\t') {
						j = 0;
						while ((i >= 0) && (txt[i] == '\t')) {
							j++;
							i--;
						}
						if ((i < 0) || (txt[i] == '\n')) {
							m_tb.SelectedText = new String('\t', j);
							return;
						}
					}
					i--;
				}
			}
		}

		private bool GetLinePos(int index, out int beg, out int len) {
			if ((index < 0) || (index >= GetLinesCount())) {
				beg = len = 0;
				return false;
			}
			int i;
			string[] ls = m_tb.Lines;
			beg = 0;
			for (i = 0; i < index; i++)
				beg += ls[i].Length + 1;
			len = ls[index].Length;
			return true;
		}

		private void SelectLine(int index) {
			int beg, len;
			if (!GetLinePos(index, out beg, out len))
				throw new IndexOutOfRangeException();
			m_tb.SelectionStart = beg;
			m_tb.SelectionLength = len;
		}

		private void HighlightLine(int index) {
			int beg, len;
			int curbeg = m_tb.SelectionStart;
			int curlen = m_tb.SelectionLength;
			GetLinePos(index, out beg, out len);
			string str = m_tb.Lines[index];
			m_hp.Reset(str);
			while (m_hp.HasMore()) {
				int tbeg, tlen;
				HighlightParser.TokenType type;
				m_hp.GetNext(out tbeg, out tlen, out type);
				m_tb.SelectionStart = beg + tbeg;
				m_tb.SelectionLength = tlen;
				switch (type) {
					case HighlightParser.TokenType.Comment:
						{
							m_tb.SelectionColor = Color.DarkGreen;
							break;
						}
					case HighlightParser.TokenType.Identifier:
						{
							m_tb.SelectionColor = Color.Purple;
							break;
						}
					case HighlightParser.TokenType.Keyword:
						{
							m_tb.SelectionColor = Color.Blue;
							break;
						}
					case HighlightParser.TokenType.Number:
						{
							m_tb.SelectionColor = Color.Red;
							break;
						}
					case HighlightParser.TokenType.String:
						{
							m_tb.SelectionColor = Color.Brown;
							break;
						}
					case HighlightParser.TokenType.Other:
						{
							m_tb.SelectionColor = Color.Black;
							break;
						}
				}
			}
			m_tb.SelectionStart = curbeg;
			m_tb.SelectionLength = curlen;
		}

		public enum LineState {
			ErrorLine,
			CurrentLine,
			NormalLine
		}

		private void ColorLine(int index, LineState state) {
			int curbeg = m_tb.SelectionStart;
			int curlen = m_tb.SelectionLength;
			SelectLine(index);
			switch (state) {
				case LineState.ErrorLine:
					{
						m_tb.SelectionColor = Color.Red;
						break;
					}
				case LineState.CurrentLine:
					{
						m_tb.SelectionFont = s_cfont;
						break;
					}
				case LineState.NormalLine:
					{
						m_tb.SelectionFont = s_nfont;
						HighlightLine(index);
						break;
					}
			}
			m_tb.SelectionStart = curbeg;
			m_tb.SelectionLength = curlen;
		}

		private void HighlightText(bool anyway) {
			int l = GetCurrentLine();
			int lc = GetLinesCount();
			if ((l != m_curline) || (lc != m_lincount) || anyway) {
				m_tb_2.Focus();
				m_curline = l;
				m_lincount = lc;
				int bi = m_tb.GetCharIndexFromPosition(new Point(0, 0));
				int ei = m_tb.GetCharIndexFromPosition(new Point(m_tb.Size));
				int bl = m_tb.GetLineFromCharIndex(bi);
				int el = m_tb.GetLineFromCharIndex(ei);
				if (bl > 0) bl--;
				if (el < lc) el++;
				for (int i = bl; i < el; i++)
					HighlightLine(i);
				m_tb.Focus();
			}
		}

		private void m_tb_KeyUp(object sender, KeyEventArgs e) {
			HighlightText(false);
		}

		private void m_tb_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left)
				HighlightText(true);
		}

		public string[] Lines {
			get { return (string[]) m_tb.Lines.Clone(); }
		}

		public bool LoadFile(string filename) {
			try {
				m_tb.LoadFile(filename, RichTextBoxStreamType.PlainText);
				HighlightText(true);
				return true;
			}
			catch {
				return false;
			}
		}

		public bool SaveFile(string filename) {
			try {
				m_tb.SaveFile(filename, RichTextBoxStreamType.PlainText);
				return true;
			}
			catch {
				return false;
			}
		}

		public int CurrentLine {
			get { return m_tb.GetLineFromCharIndex(m_tb.SelectionStart); }
		}


		private class HighlightParser {
			private char[] m_a;
			private int m_len;
			private int m_cur;

			public enum TokenType {
				String,
				Number,
				Keyword,
				Comment,
				Identifier,
				Other
			}

			public void Reset(string str) {
				m_a = str.ToCharArray();
				m_len = str.Length;
				m_cur = 0;
				while ((m_cur < m_len) && Char.IsWhiteSpace(m_a[m_cur]))
					m_cur++;
			}

			public bool HasMore() {
				return m_cur < m_len;
			}

			private bool IsKeyword(string str) {
				return
					(str == "if") ||
						(str == "else") ||
						(str == "elseif") ||
						(str == "endif") ||
						(str == "while") ||
						(str == "loop") ||
						(str == "return") ||
						(str == "result") ||
						(str == "call") ||
						(str == "print") ||
						(str == "println") ||
						(str == "readln") ||
						(str == "clear") ||
						(str == "for") ||
						(str == "next") ||
						(str == "error");
			}

			public void GetNext(out int beg, out int len, out TokenType type) {
				if (m_cur >= m_len)
					throw new IndexOutOfRangeException();
				beg = m_cur;
				if (m_a[m_cur] == '\"') {
					m_cur++;
					while ((m_cur < m_len) && (m_a[m_cur] != '\"'))
						m_cur++;
					if (m_cur < m_len)
						m_cur++;
					len = m_cur - beg;
					type = TokenType.String;
				}
				else if (isL(m_a[m_cur])) {
					m_cur++;
					while ((m_cur < m_len) && isLD(m_a[m_cur]))
						m_cur++;
					len = m_cur - beg;
					if (IsKeyword(new string(m_a, beg, len)))
						type = TokenType.Keyword;
					else
						type = TokenType.Identifier;
				}
				else if (m_a[m_cur] == '#') {
					len = m_len - m_cur;
					m_cur = m_len;
					type = TokenType.Comment;
				}
				else if (m_a[m_cur] == '.') {
					if (GetNumber()) {
						len = m_cur - beg;
						type = TokenType.Number;
					}
					else {
						m_cur = beg + 1;
						len = 1;
						type = TokenType.Other;
					}
				}
				else if (char.IsDigit(m_a[m_cur])) {
					GetNumber();
					len = m_cur - beg;
					type = TokenType.Number;
				}
				else {
					m_cur++;
					len = 1;
					type = TokenType.Other;
				}
				while ((m_cur < m_len) && Char.IsWhiteSpace(m_a[m_cur]))
					m_cur++;
			}

			private bool GetNumber() {
				if (!((m_a[m_cur] == '.') || char.IsDigit(m_a[m_cur])))
					return false;
				while ((m_cur < m_len) && char.IsDigit(m_a[m_cur]))
					m_cur++;
				if (m_cur == m_len)
					return true;
				else if (m_a[m_cur] == '.') {
					m_cur++;
					while ((m_cur < m_len) && char.IsDigit(m_a[m_cur]))
						m_cur++;
					if (m_cur == m_len)
						return true;
					else if ((m_a[m_cur] == 'e') || (m_a[m_cur] == 'E')) {
						int p1 = m_cur;
						m_cur++;
						if (m_cur == m_len) {
							m_cur = p1;
							return true;
						}
						else if ((m_a[m_cur] == '-') || (m_a[m_cur] == '+')) {
							m_cur++;
							if ((m_cur == m_len) || !char.IsDigit(m_a[m_cur])) {
								m_cur = p1;
								return true;
							}
							while ((m_cur < m_len) && char.IsDigit(m_a[m_cur]))
								m_cur++;
							return true;
						}
						else if (char.IsDigit(m_a[m_cur])) {
							while ((m_cur < m_len) && char.IsDigit(m_a[m_cur]))
								m_cur++;
							return true;
						}
						else {
							m_cur = p1;
							return true;
						}
					}
					else
						return true;
				}
				else if ((m_a[m_cur] == 'e') || (m_a[m_cur] == 'E')) {
					int p1 = m_cur;
					m_cur++;
					if (m_cur == m_len) {
						m_cur = p1;
						return true;
					}
					else if ((m_a[m_cur] == '-') || (m_a[m_cur] == '+')) {
						m_cur++;
						if ((m_cur == m_len) || !char.IsDigit(m_a[m_cur])) {
							m_cur = p1;
							return true;
						}
						while ((m_cur < m_len) && char.IsDigit(m_a[m_cur]))
							m_cur++;
						return true;
					}
					else if (char.IsDigit(m_a[m_cur])) {
						while ((m_cur < m_len) && char.IsDigit(m_a[m_cur]))
							m_cur++;
						return true;
					}
					else {
						m_cur = p1;
						return true;
					}
				}
				else
					return true;
			}

			private static bool isLD(char c) {
				return ((c >= 'a') && (c <= 'z')) || ((c >= 'A') && (c <= 'Z')) || (c == '0')
					|| ((c >= '1') && (c <= '9')) || (c == '_');
			}

			private static bool isL(char c) {
				return ((c >= 'a') && (c <= 'z')) || ((c >= 'A') && (c <= 'Z')) || (c == '_');
			}

		}
	}
}