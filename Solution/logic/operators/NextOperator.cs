namespace interpr.logic.operators {
	public class NextOperator : IOperator {
		private int m_for_pos = -1;
		private ForOperator m_for_op = null;

		public NextOperator() {}

		public int ForPos {
			get {
				if (m_for_pos < 0)
					throw new OtherException("Error in NextOperator.ForPos");
				return m_for_pos;
			}
			set { m_for_pos = value; }
		}

		public ForOperator ForOp {
			get { return m_for_op; }
			set { m_for_op = value; }
		}

		public void Execute(interpr.logic.Subroutine.Moment pos) {
			m_for_op.Step(pos, m_for_pos);
		}

		public interpr.logic.operators.OperatorKind GetKind() {
			return OperatorKind.Next;
		}
	}
}