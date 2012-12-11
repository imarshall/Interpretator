using System;
using interpr.logic;
using interpr.logic.vartypes;

namespace interpr.logic.operators {
	public class PrintLnCommand : Command {
		private Expression m_expr;

		public PrintLnCommand(Expression expr) {
			m_expr = expr;
		}

		public override void Execute() {
			VarBase res = m_expr.Calculate();
			InterprEnvironment.Instance.CurrentConsole.PrintLn(res.ToString());
		}

	}
}