using System;
using interpr.logic;
using interpr.logic.vartypes;

namespace interpr.logic.operators {
	public class ElseifOperator : IOperator {
		private Expression m_expr;
		private int m_nextpos = -1;

		public int NextPos {
			get {
				if (m_nextpos < 0)
					throw new OtherException("Error in IfOperator.NextPos");
				return m_nextpos;
			}
			set { m_nextpos = value; }
		}

		public ElseifOperator(Expression expr) {
			m_expr = expr;
		}

		public bool TestCondition() {
			VarBase v = m_expr.Calculate();
			if (!v.IsSingle())
				throw new CalcException("Значение условия не может быть массивом");
			return (v as SingleVar).ToBool();
		}

		public void Execute(Subroutine.Moment pos) {
			int pos1 = m_nextpos;
			while (true) {
				pos.GoTo(pos1);
				if (pos.Current.GetKind() == OperatorKind.Elseif) {
					pos1 = (pos.Current as ElseifOperator).NextPos;
				}
				else if (pos.Current.GetKind() == OperatorKind.Else) {
					pos1 = (pos.Current as ElseOperator).NextPos;
				}
				else if (pos.Current.GetKind() == OperatorKind.Endif) {
					pos.Next();
					break;
				}
			}
		}

		public OperatorKind GetKind() {
			return OperatorKind.Elseif;
		}
	}
}