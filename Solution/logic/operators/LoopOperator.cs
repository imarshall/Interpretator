using System;

namespace interpr.logic.operators {
	public class LoopOperator : IOperator {
		private int m_whilepos = -1;

		public int WhilePos {
			get {
				if (m_whilepos < 0)
					throw new OtherException("Error in LoopOperator.WhilePos");
				return m_whilepos;
			}
			set { m_whilepos = value; }
		}

		public void Execute(Subroutine.Moment pos) {
			pos.GoTo(m_whilepos);
		}

		public OperatorKind GetKind() {
			return OperatorKind.Loop;
		}
	}
}