using System;
using interpr.logic;

namespace interpr.logic.operators {
	public class WhileOperator : IOperator {
		private Expression m_expr;
		private int m_looppos = -1;

		public WhileOperator(Expression expr) {
			m_expr = expr;
		}

		public int LoopPos {
			get {
				if (m_looppos < 0)
					throw new OtherException("Error in WhileOperator.LoopPos");
				return m_looppos;
			}
			set { m_looppos = value; }
		}

		public void Execute(interpr.logic.Subroutine.Moment pos) {
			vartypes.VarBase v = m_expr.Calculate();
			if (!v.IsSingle())
				throw new CalcException("Значение условия не может быть массивом");
			if ((v as vartypes.SingleVar).ToBool())
				pos.Next();
			else
				pos.GoTo(m_looppos + 1);
		}

		public interpr.logic.operators.OperatorKind GetKind() {
			return OperatorKind.While;
		}

	}
}