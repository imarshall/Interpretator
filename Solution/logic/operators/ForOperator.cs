using interpr.logic.vartypes;

namespace interpr.logic.operators {
	public class ForOperator : IOperator {
		private int m_next_pos = -1;
		private string m_counter_var = null;
		private Expression m_begin = null;
		private Expression m_end = null;
		private IntVar m_end_res = null;

		public ForOperator(string counter, Expression beg, Expression end) {
			m_counter_var = counter;
			m_begin = beg;
			m_end = end;
		}

		public int NextPos {
			get {
				if (m_next_pos < 0)
					throw new OtherException("Error in LoopOperator.NextPos");
				return m_next_pos;
			}
			set { m_next_pos = value; }
		}

		public void Step(Subroutine.Moment pos, int forpos) {
			Namespace cn = InterprEnvironment.Instance.CurrentNamespace;
			VarBase res = cn[m_counter_var];
			if (!res.IsInt())
				throw new CalcException("Тип переменной - счетчика цикла был изменен");
			int resval = (res as IntVar).Val;
			resval++;
			res = new IntVar(resval);
			cn[m_counter_var] = res;
			if (resval > m_end_res.Val)
				pos.GoTo(m_next_pos + 1);
			else
				pos.GoTo(forpos + 1);
		}

		public void Execute(Subroutine.Moment pos) {
			VarBase resb, rese;
			resb = m_begin.Calculate();
			if (!resb.IsInt())
				throw new CalcException("Границы изменения счетчика должны быть целыми");
			IntVar resbi = resb as IntVar;
			Namespace cn = InterprEnvironment.Instance.CurrentNamespace;
			cn[m_counter_var] = resb;
			rese = m_end.Calculate();
			if (!rese.IsInt())
				throw new CalcException("Границы изменения счетчика должны быть целыми");
			m_end_res = rese as IntVar;
			if (resbi.Val > m_end_res.Val)
				pos.GoTo(m_next_pos + 1);
			else
				pos.Next();
		}

		public OperatorKind GetKind() {
			return OperatorKind.For;
		}

	}
}