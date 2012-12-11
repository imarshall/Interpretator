using System;

namespace interpr.logic.operators {
	public class CallCommand : Command {
		private Expression m_expr;

		public CallCommand(Expression expr) {
			m_expr = expr;
		}

		public override void Execute() {
			m_expr.Calculate(); //результат игнорируется
		}
	}
}