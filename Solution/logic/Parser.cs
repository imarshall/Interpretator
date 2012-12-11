using System;
using System.Collections;

namespace interpr.logic {
	public class Parser : IEnumerable, IEnumerator {
		private char[] m_a;
		private int m_len;
		private int m_cur = 0;
		private int m_new_cur = -1;
		private bool m_at_begin;

		private static readonly string[] s_keywords =
			new string[] {
				"if",
				"else",
				"elseif",
				"endif",
				"while",
				"loop",
				"return",
				"call",
				"print",
				"println",
				"readln",
				"clear",
				"for",
				"next",
				"error"
			};

		private static readonly int s_keywords_length = s_keywords.Length;

		private static bool IsLD(char c) {
			return ((c >= 'a') && (c <= 'z')) || ((c >= 'A') && (c <= 'Z')) || (c == '0')
				|| ((c >= '1') && (c <= '9')) || (c == '_');
		}

		private static bool IsSp(char c) {
			return (c == ' ') || (c == '\t');
		}

		public static bool IsID(string str) {
			int l = str.Length;
			if (l == 0)
				return false;
			if (char.IsDigit(str[0]) || (!IsLD(str[0])))
				return false;
			int i;
			for (i = 1; i < str.Length; i++)
				if (!IsLD(str[i]))
					return false;
			for (i = 0; i < s_keywords_length; i++)
				if (str == s_keywords[i])
					return false;
			return true;
		}


		public void Reset() {
			m_cur = 0;
			m_new_cur = -1;
			m_at_begin = true;
		}

		public string GetString() {
			return new String(m_a, 0, m_len);
		}


		public bool HasMore() {
			return m_cur < m_len;
		}


		public Parser(string str) {
			char[] a = str.ToCharArray();
			int n = a.Length;
			int i = 0;
			int j = 0;

			m_a = new char[n];
			while (i < n) {
				if (a[i] == '#') {
					break;
				} else if (a[i] == '\"') {
					m_a[j] = '\"';
					i++;
					j++;
					while ((i < n) && (a[i] != '\"')) {
						m_a[j] = a[i];
						i++;
						j++;
					}
					if (i == n)
						throw new SyntaxErrorException("Не закрытая строковая константа");
					else {
						m_a[j] = '\"';
						i++;
						j++;
					}
				} else if (IsSp(a[i])) {
					bool flag = false;
					if ((i > 0) && (IsLD(a[i - 1]))) {
						m_a[j] = ' ';
						j++;
						flag = true;
					}
					while ((i < n) && IsSp(a[i]))
						i++;
					if (((i == n) || (!IsLD(a[i]))) && flag)
						j--;
				} else {
					m_a[j] = a[i];
					i++;
					j++;
				}
			}
			m_len = j;
			Reset();
		}

		private string GetCurrent() {
			int cur = m_cur;
			int beg = m_cur;
			int end = m_len;
			string res = null;
			bool flag = true;
			if ((m_a[cur] == '.') && ((cur < end - 1) && (!char.IsDigit(m_a[cur + 1]))) || (cur == end - 1)) {
				flag = true;
			} else if (char.IsDigit(m_a[cur]) || (m_a[cur] == '.')) {
				flag = false;
				while ((cur < end) && char.IsDigit(m_a[cur]))
					cur++;
				if (cur == end) {
					res = new String(m_a, beg, cur - beg);
				} else if ((m_a[cur] == 'e') || (m_a[cur] == 'E')) {
					cur++;
					if (cur == end) {
						cur--;
						res = new String(m_a, beg, cur - beg);
					} else if ((m_a[cur] == '+') || (m_a[cur] == '-')) {
						cur++;
						if ((cur == end) || (!char.IsDigit(m_a[cur]))) {
							cur -= 2;
							res = new String(m_a, beg, cur - beg);
						}
						while ((cur < end) && char.IsDigit(m_a[cur]))
							cur++;
						res = new String(m_a, beg, cur - beg);
					} else if (char.IsDigit(m_a[cur])) {
						while ((cur < end) && char.IsDigit(m_a[cur]))
							cur++;
						res = new String(m_a, beg, cur - beg);
					} else {
						cur--;
						res = new String(m_a, beg, cur - beg);
					}
				} else if (m_a[cur] == '.') {
					cur++;
					if ((cur == end) || (!char.IsDigit(m_a[cur]))) {
						cur--;
						res = new String(m_a, beg, cur - beg);
					} else {
						while ((cur < end) && char.IsDigit(m_a[cur]))
							cur++;
						if (cur == end)
							res = new String(m_a, beg, cur - beg);
						else if ((m_a[cur] == 'e') || (m_a[cur] == 'E')) {
							cur++;
							if (cur == end) {
								cur--;
								res = new String(m_a, beg, cur - beg);
							} else if ((m_a[cur] == '+') || (m_a[cur] == '-')) {
								cur++;
								if ((cur == end) || (!char.IsDigit(m_a[cur]))) {
									cur -= 2;
									res = new String(m_a, beg, cur - beg);
								}
								while ((cur < end) && char.IsDigit(m_a[cur]))
									cur++;
								res = new String(m_a, beg, cur - beg);
							} else if (char.IsDigit(m_a[cur])) {
								while ((cur < end) && char.IsDigit(m_a[cur]))
									cur++;
								res = new String(m_a, beg, cur - beg);
							} else {
								cur--;
								res = new String(m_a, beg, cur - beg);
							}
						} else
							res = new String(m_a, beg, cur - beg);
					}
				} else
					res = new String(m_a, beg, cur - beg);
			}
			if (flag) {
				if (IsLD(m_a[cur])) {
					while ((cur < end) && IsLD(m_a[cur]))
						cur++;
					res = new String(m_a, beg, cur - beg);
				} else if (m_a[cur] == '\"') {
					do {
						cur++;
						if (m_a[cur] == '\"') {
							if ((cur < end - 1) && (m_a[cur + 1] == '\"'))
								cur++;
							else
								break;
						}
					} while (true);
					cur++;
					res = new String(m_a, beg, cur - beg);
				} else if (cur < end - 1) {
					switch (m_a[cur]) {
						case ':':
							{
								cur++;
								if (m_a[cur] == '=') {
									cur++;
									res = ":=";
								} else
									res = ":";
								break;
							}
						case '~':
							{
								cur++;
								if (m_a[cur] == '=') {
									cur++;
									res = "~=";
								} else
									res = "~";
								break;
							}
						case '>':
							{
								cur++;
								if (m_a[cur] == '=') {
									cur++;
									res = ">=";
								} else
									res = ">";
								break;
							}
						case '<':
							{
								cur++;
								switch (m_a[cur]) {
									case '=':
										{
											cur++;
											res = "<=";
											break;
										}
									case '>':
										{
											cur++;
											res = "<>";
											break;
										}
									default:
										{
											res = "<";
											break;
										}
								}
								break;
							}
						default:
							{
								res = m_a[cur].ToString();
								cur++;
								break;
							}
					}
				} else {
					res = m_a[cur].ToString();
					cur++;
				}
			}
			if ((cur < end) && IsSp(m_a[cur]))
				cur++;
			m_new_cur = cur;
			return res;
		}

		public object Current {
			get { return GetCurrent(); }
		}

		public bool MoveNext() {
			if (m_at_begin) {
				m_at_begin = false;
				return HasMore();
			}
			if (m_new_cur < 0)
				GetCurrent();
			m_cur = m_new_cur;
			m_new_cur = -1;
			return HasMore();
		}

		public IEnumerator GetEnumerator() {
			return this;
		}

		public static bool IsUserID(string name) {
			if (!IsID(name))
				return false;
			if (name == "abs")
				return false;
			if (name == "cos")
				return false;
			if (name == "sin")
				return false;
			if (name == "tg")
				return false;
			if (name == "arccos")
				return false;
			if (name == "arcsin")
				return false;
			if (name == "arctg")
				return false;
			if (name == "exp")
				return false;
			if (name == "pow")
				return false;
			if (name == "ln")
				return false;
			if (name == "lg")
				return false;
			if (name == "log")
				return false;
			if (name == "sqrt")
				return false;
			if (name == "pi")
				return false;
			if (name == "idiv")
				return false;
			if (name == "iff")
				return false;
			if (name == "imod")
				return false;
			if (name == "random")
				return false;
			if (name == "substr")
				return false;
			if (name == "strlen")
				return false;
			if (name == "strpos")
				return false;
			if (name == "toint")
				return false;
			if (name == "toreal")
				return false;
			if (name == "tostring")
				return false;
			if (name == "isarray")
				return false;
			if (name == "issingle")
				return false;
			if (name == "isstring")
				return false;
			if (name == "isnum")
				return false;
			if (name == "isreal")
				return false;
			if (name == "isint")
				return false;
			if (name == "size")
				return false;
			return true;
		}
	}
}