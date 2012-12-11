using System;

namespace interpr.logic.operators {
	public class ClearCommand : Command {
		private string m_name;

		public ClearCommand(string name) {
			m_name = name;
		}

		public override void Execute() {
			InterprEnvironment.Instance.CurrentNamespace.Remove(m_name);
		}

	}
}