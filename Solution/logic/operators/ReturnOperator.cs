using System;

namespace interpr.logic.operators {
	public class ReturnOperator : IOperator {
		public ReturnOperator() {}

		public void Execute(Subroutine.Moment pos) {
			pos.Return();
		}

		public OperatorKind GetKind() {
			return OperatorKind.Return;
		}
	}
}