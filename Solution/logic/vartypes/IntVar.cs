using System;
using System.IO;

namespace interpr.logic.vartypes {
	public class IntVar : NumVar {
		public virtual int Val {
			get { return m_val; }

			set { m_val = value; }

		}

		private int m_val = 0;//значение

		public IntVar() {}

		public IntVar(int val) {
			m_val = val;
		}

		public IntVar(bool val) {
			m_val = val ? 1 : - 1;
		}

		public override System.String ToString() {
			return m_val.ToString();
		}


		public override bool ToBool() {
			return m_val > 0;
		}

		public override double ToDouble() {
			return (double) m_val;
		}

		public override SingleVar add(SingleVar b) {
			if (b.IsInt())
				return new IntVar(this.m_val + ((IntVar) b).Val);
			else if (b.IsReal()) {
				try {
					return new RealVar((double) this.m_val + ((RealVar) b).Val);
				}
				catch (System.SystemException exc) {
					throw new CalcException("Ошибка в вычислениях.");
				}
			}
			else
				return null;
		}

		public override NumVar sub(NumVar b) {
			if (b.IsInt())
				return new IntVar(this.m_val - ((IntVar) b).Val);
			else if (b.IsReal()) {
				try {
					return new RealVar((double) this.m_val - ((RealVar) b).Val);
				}
				catch (System.SystemException exc) {
					throw new CalcException("Ошибка в вычислениях.");
				}
			}
			else
				return null;
		}

		public override NumVar mul(NumVar b) {
			if (b.IsInt())
				return new IntVar(this.m_val*((IntVar) b).Val);
			else if (b.IsReal()) {
				try {
					return new RealVar((double) this.m_val*((RealVar) b).Val);
				}
				catch (System.SystemException exc) {
					throw new CalcException("Ошибка в вычислениях.");
				}
			}
			else
				return null;
		}

		public override System.Object Clone() {
			return new IntVar(m_val);
		}

		public override void Serialise(BinaryWriter bw) {
			bw.Write('i');
			bw.Write(m_val);
		}

		public override bool Equals(System.Object obj) {
			if (obj is RealVar)
				return ((RealVar) obj).ToDouble() == this.Val;
			else if(obj is IntVar)
				return (obj is IntVar) && (((IntVar) obj).Val == m_val);
			return false;
		}

	}
}