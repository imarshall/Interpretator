namespace interpr.logic.operators {

	public abstract class Command : IOperator {

		public abstract void Execute();

		public void Execute(Subroutine.Moment pos) {
			Execute();
			pos.Next();
		}

		public OperatorKind GetKind() {
			return OperatorKind.Plain;
		}

	}
}