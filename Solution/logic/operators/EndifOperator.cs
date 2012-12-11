using System;

namespace interpr.logic.operators {
	public class EndifOperator : IOperator {
		public EndifOperator() {}

		public void Execute(Subroutine.Moment pos) {
			pos.Next();
		}

		public OperatorKind GetKind() {
			return OperatorKind.Endif;
		}
	}
}