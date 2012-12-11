namespace interpr.logic.operators {
	public enum OperatorKind {
		Plain,
		If,
		Elseif,
		Else,
		Endif,
		While,
		Loop,
		For,
		Next,
		Return
	}

	public interface IOperator {
		void Execute(Subroutine.Moment pos);
		OperatorKind GetKind();
	}
}