using System;

namespace interpr.logic.vartypes {
	public abstract class NumVar : SingleVar {
		public abstract double ToDouble();

		public override bool MoreThan(SingleVar b) {
			if (!b.IsNum())
				throw new CalcException("Неверные типы операндов");
			return this.ToDouble() > ((NumVar) b).ToDouble();
		}

		public abstract NumVar sub(NumVar b);

		public abstract NumVar mul(NumVar b);

		public virtual NumVar div(NumVar b) {
			try {
				return new RealVar(this.ToDouble()/b.ToDouble());
			}
			catch {
				throw new CalcException("Ошибка в вычислениях");
			}
		}
	}
}