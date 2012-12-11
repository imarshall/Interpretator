using System;

namespace interpr.logic.operators {
	public class ElseOperator : IOperator {
		private int m_nextpos = -1;

		public int NextPos {
			get {
				if (m_nextpos < 0)
					throw new OtherException("Error in IfOperator.NextPos");
				return m_nextpos;
			}
			set { m_nextpos = value; }
		}

		public void Execute(Subroutine.Moment pos) {
			pos.GoTo(m_nextpos + 1);
		}

		public OperatorKind GetKind() {
			return OperatorKind.Else;
		}
	}
}