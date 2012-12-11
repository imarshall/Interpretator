using System;
using System.IO;

namespace interpr.logic.vartypes {
	public class RealVar : NumVar {
		public virtual double Val {
			get { return m_val; }

			set {
				if ((System.Double.IsInfinity(value)) || (System.Double.IsNaN(value)))
					throw new System.SystemException("Invalid real value");
				m_val = value;
			}

		}

		private double m_val = 0.0; //значение

		public RealVar() {}

		public RealVar(double val) {
			if ((System.Double.IsInfinity(val)) || (System.Double.IsNaN(val)))
				throw new System.SystemException("Invalid real value");
			m_val = val;
		}

		public override System.String ToString() {
			return m_val.ToString();
		}


		public override bool ToBool() {
			return m_val > 0;
		}

		public override double ToDouble() {
			return m_val;
		}

		public override SingleVar add(SingleVar b) {
			if (b.IsNum()) {
				try {
					return new RealVar(this.m_val + ((NumVar) b).ToDouble());
				}
				catch (Exception exc) {
					throw new CalcException("Ошибка в вычислениях.");
				}
			}
			else
				throw new CalcException("Неправильные операнды.");
		}

		public override NumVar sub(NumVar b) {
			try {
				return new RealVar(this.m_val - ((NumVar) b).ToDouble());
			}
			catch (Exception exc) {
				throw new CalcException("Ошибка в вычислениях.");
			}
		}

		public override NumVar mul(NumVar b) {
			try {
				return new RealVar(this.m_val*((NumVar) b).ToDouble());
			}
			catch (Exception exc) {
				throw new CalcException("Ошибка в вычислениях.");
			}
		}

		public override System.Object Clone() {
			return new RealVar(m_val);
		}

		public override void Serialise(BinaryWriter bw) {
			bw.Write('r');
			bw.Write(m_val);
		}

		public override bool Equals(System.Object obj) {
			if (obj is NumVar)
				return this.ToDouble() == ((NumVar) obj).ToDouble();
			else
				return false;
		}

	}
}