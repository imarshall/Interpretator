using System.Threading;
using interpr.logic;
using interpr.logic.operators;

namespace interpr {
	public class Facade {
		private static Facade s_instance = null;

		public static void Create(IConsole console) {
			if (s_instance == null)
				s_instance = new Facade(console);
		}

		public static Facade Instance {
			get { return s_instance; }
		}

		private IConsole m_console;
		private InterprEnvironment m_env;
		private string m_cmd;
		private bool m_doing = false;

		private Facade(IConsole console) {
			m_console = console;
			m_env = InterprEnvironment.Instance;
			m_env.CurrentConsole = m_console;
		}

		public delegate void CommandDoneHandler();
		public event CommandDoneHandler Done;

		private void ThrStart() {
			m_doing = true;
			Command cmd;
			do {
				try {
					cmd = LineCompiler.CompileCommand(m_cmd);
				}
				catch (SyntaxErrorException ex) {
					m_env.CurrentConsole.PrintLn("Ошибка : " + ex.Message);
					break;
				}
				try {
					cmd.Execute();
				}
				catch (CalcException ex) {
					m_env.CurrentConsole.PrintLn("Ошибка : " + ex.Message);
					m_env.CurrentNamespace = m_env.ConsoleNamespace;
					break;
				}
			} while (false);
			Done();
			m_doing = false;
		}

		public void ExecuteCommand(string cmd) {
			if (m_doing)
				throw new OtherException("Error in Bridge.ExecuteCommand()");
			m_cmd = cmd;
			new Thread(new ThreadStart(ThrStart)).Start();
		}

		private void DoRestart() {
			if (m_doing)
				Subroutine.Moment.Break();
			while (m_doing) {}
			InterprEnvironment.Reset();
			m_env = InterprEnvironment.Instance;
			m_env.CurrentConsole = m_console;
			m_env.LoadSubs();
		}

		public void Restart() {
			new Thread(new ThreadStart(DoRestart)).Start();
		}

		public bool Busy {
			get { return m_doing; }
		}

		public void SaveVariables() {
			m_env.SaveVars();
		}

		public void LoadSubs() {
			m_env.LoadSubs();
		}

		public ConsoleNamespace.VariableReport[] GetVariables() {
			return m_env.GetGlobalVarsList();
		}

		public string[] GetSubs() {
			return m_env.LoadedSubs;
		}

		public void DeleteVariable(string name) {
			m_env.ConsoleNamespace.Remove(name);
		}

		public bool LoadSub(string name) {
			return m_env.LoadSub(name);
		}

		public void UnloadSub(string name) {
			m_env.UnloadSub(name);
		}

		public bool NotRestored {
			get {
				return m_env.NotRestored;
			}
		}
	}
}