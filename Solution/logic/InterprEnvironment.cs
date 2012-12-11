using System;
using System.Collections;
using System.IO;

namespace interpr.logic {
	public class InterprEnvironment {
		private SubroutinesManager m_subsman = null;

		private ConsoleNamespace m_console_vars;
		private bool m_not_restored = false;

		public bool NotRestored {
			get { return m_not_restored; }
		}

		public ConsoleNamespace ConsoleNamespace {
			get { return m_console_vars; }
		}

		public ConsoleNamespace.VariableReport[] GetGlobalVarsList() {
			return m_console_vars.GetVariableList();
		}

		private InterprEnvironment() {
			m_current_namespace = new ConsoleNamespace();
			m_console_vars = m_current_namespace as ConsoleNamespace;
			m_not_restored = false;
			try {
				m_console_vars.Restore();
			} catch {
				m_not_restored = true;
				m_console_vars = new ConsoleNamespace();
				m_current_namespace = m_console_vars;
			}
		}

		public void LoadSubs() {
			if (m_current_console == null)
				throw new OtherException("Error in Environment.LoadSubs()");
			s_instance.m_subsman = SubroutinesManager.GetInstance();
			s_instance.m_subsman.ReloadAll();
		}

		private static InterprEnvironment s_instance = null;

		public static InterprEnvironment Instance {
			get {
				if (s_instance == null)
					s_instance = new InterprEnvironment();
				return s_instance;
			}
		}

		public static void Reset() {
			s_instance = new InterprEnvironment();
		}

		public void SaveVars() {
			m_console_vars.Save();
		}

		public bool LoadSub(string name) {
			return m_subsman.Load(name);
		}

		private Namespace m_current_namespace = null;

		public Namespace CurrentNamespace {
			get { return m_current_namespace; }
			set { m_current_namespace = value; }
		}

		private IConsole m_current_console = null;

		public IConsole CurrentConsole {
			get { return m_current_console; }
			set { m_current_console = value; }
		}

		public Operation GetFunction(string name) {
			if (name == "abs")
				return Operation.ABS;
			if (name == "cos")
				return Operation.COS;
			if (name == "sin")
				return Operation.SIN;
			if (name == "tg")
				return Operation.TG;
			if (name == "arccos")
				return Operation.ARCCOS;
			if (name == "arcsin")
				return Operation.ARCSIN;
			if (name == "arctg")
				return Operation.ARCTG;
			if (name == "defined")
				return Operation.DEFINED;
			if (name == "exp")
				return Operation.EXP;
			if (name == "pow")
				return Operation.POW;
			if (name == "ln")
				return Operation.LN;
			if (name == "lg")
				return Operation.LG;
			if (name == "log")
				return Operation.LOG;
			if (name == "sqrt")
				return Operation.SQRT;
			if (name == "pi")
				return Operation.PI;
			if (name == "idiv")
				return Operation.IDIV;
			if (name == "iff")
				return Operation.IFF;
			if (name == "imod")
				return Operation.IMOD;
			if (name == "random")
				return Operation.RANDOM;
			if (name == "substr")
				return Operation.SUBSTR;
			if (name == "strlen")
				return Operation.STRLEN;
			if (name == "strpos")
				return Operation.STRPOS;
			if (name == "toint")
				return Operation.TOINT;
			if (name == "toreal")
				return Operation.TOREAL;
			if (name == "tostring")
				return Operation.TOSTRING;
			if (name == "isarray")
				return Operation.ISARRAY;
			if (name == "issingle")
				return Operation.ISSINGLE;
			if (name == "isstring")
				return Operation.ISSTRING;
			if (name == "isnum")
				return Operation.ISNUM;
			if (name == "isreal")
				return Operation.ISREAL;
			if (name == "isint")
				return Operation.ISINT;
			if (name == "size")
				return Operation.SIZE;
			return new SubName(name);
		}

		public string[] LoadedSubs {
			get { return m_subsman.SubroutineNames; }
		}

		private class SubroutinesManager {
			private ArrayList m_subs = new ArrayList();
			private ArrayList m_names = new ArrayList();

			private SubroutinesManager() {
				DirectoryInfo di =
					new DirectoryInfo(Directory.GetCurrentDirectory() + @"\subroutines");
				if (!di.Exists) {
					di.Create();
				}
			}

			public bool Load(string name) {
				FileInfo fi = new FileInfo(Directory.GetCurrentDirectory() + @"\subroutines\" + name);
				if (!fi.Exists)
					throw new OtherException("Error in SubroutinesManager.Load()");
				return LoadFile(fi);
			}

			private bool LoadFile(FileInfo file) {
				try {
					StreamReader sr = file.OpenText();
					LinkedList ll = new LinkedList();
					try {
						while (sr.Peek() != -1) {
							ll.AddFirst(sr.ReadLine());
						}
					} finally {
						sr.Close();
					}
					string[] strs = new String[ll.Count];
					int i = 0;
					while (!ll.IsEmpty()) {
						strs[i] = (ll.RemoveLast() as String);
						i++;
					}
					Subroutine sub;
					try {
						sub = new Subroutine(strs, file.Name);
					} catch (LineSyntaxException ex) {
						InterprEnvironment.Instance.CurrentConsole.PrintLn("Синтаксическая ошибка в " + ex.Function + "[] at line " + ex.Line + " " + ex.Message);
						return false;
					} catch (SyntaxErrorException ex) {
						InterprEnvironment.Instance.CurrentConsole.PrintLn("Синтаксическая ошибка в " + file.Name + " " + ex.Message);
						return false;
					}
					Set(file.Name, sub);
				} catch {
					throw new OtherException("Error in Environment.Load()");
				}
				return true;
			}

			public Subroutine this[string name] {
				get {
					int sres = m_names.IndexOf(name);
					if (sres < 0)
						return null;
					else
						return m_subs[sres] as Subroutine;
				}
			}

			private void Set(string name, Subroutine sub) {
				int sres = m_names.IndexOf(name);
				if (sres >= 0) {
					m_names.RemoveAt(sres);
					m_subs.RemoveAt(sres);
				}
				m_names.Add(name);
				m_subs.Add(sub);
			}

			private static SubroutinesManager s_inst = null;

			public static SubroutinesManager GetInstance() {
				if (s_inst == null)
					s_inst = new SubroutinesManager();
				return s_inst;
			}

			public string[] SubroutineNames {
				get {
					int count = m_names.Count;
					string[] res = new string[count];
					for (int i = 0; i < count; i++) {
						res[i] = (m_names[i] as String);
					}
					for (int i = 0; i < count - 1; i++) {
						int k = i;
						for (int j = i + 1; j < count; j++)
							k = (string.Compare(res[j], res[k]) < 0) ? j : k;
						if (i != k) {
							string temp = res[i];
							res[i] = res[k];
							res[k] = temp;
						}
					}
					return res;
				}
			}

			public void ReloadAll() {
				m_subs = new ArrayList();
				m_names = new ArrayList();
				DirectoryInfo di =
					new DirectoryInfo(Directory.GetCurrentDirectory() + @"\subroutines");
				if (!di.Exists) {
					di.Create();
				}
				foreach (FileInfo file in di.GetFiles()) {
					if (Parser.IsID(file.Name)) {
						LoadFile(file);
					}
				}
			}

			public void Unload(string name) {
				int index = m_names.IndexOf(name);
				if (index >= 0) {
					m_names.RemoveAt(index);
					m_subs.RemoveAt(index);
				}
			}
		}

		public Subroutine GetSub(string name) {
			Subroutine res = m_subsman[name];
			if (res == null)
				throw new CalcException("Функция " + name + " не существует");
			return res;
		}

		public void UnloadSub(string name) {
			m_subsman.Unload(name);
		}
	}
}