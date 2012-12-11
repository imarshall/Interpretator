using System;

namespace interpr.logic.operators {
	public class ErrorOperator : IOperator {
		public ErrorOperator() {}

		public OperatorKind GetKind() {
			return OperatorKind.Plain;
		}

		public void Execute(Subroutine.Moment moment) {
			throw new CalcException("Выполнение прервано оператором error");
		}

	}
}