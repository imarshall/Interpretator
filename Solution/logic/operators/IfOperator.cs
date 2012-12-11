using System;
using System.Security;
using interpr.logic;
using interpr.logic.vartypes;

namespace interpr.logic.operators {
	public class IfOperator : IOperator {
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

		public IfOperator(Expression expr) {
			m_expr = expr;
		}

		public void Execute(Subroutine.Moment pos) {
			VarBase v = m_expr.Calculate();
			if (!v.IsSingle())
				throw new CalcException("Значение условия не может быть массивом");
			if ((v as SingleVar).ToBool()) {
				pos.Next();
			}
			else {
				int pos1 = m_nextpos;
				while (true) {
					pos.GoTo(pos1);
					if (pos.Current.GetKind() == OperatorKind.Else) {
						pos.Next();
						break;
					}
					else if (pos.Current.GetKind() == OperatorKind.Elseif) {
						if ((pos.Current as ElseifOperator).TestCondition()) {
							pos.Next();
							break;
						}
						pos1 = (pos.Current as ElseifOperator).NextPos;
					}
					else if (pos.Current.GetKind() == OperatorKind.Endif) {
						pos.Next();
						break;
					}
				}
			}

		}

		public OperatorKind GetKind() {
			return OperatorKind.If;
		}
	}
}