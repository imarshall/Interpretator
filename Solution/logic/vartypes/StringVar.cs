using System;
using System.IO;

namespace interpr.logic.vartypes {
	public class StringVar : SingleVar {
		public virtual System.String Val {
			get { return m_val; }

			set { m_val = value; }

		}

		private System.String m_val = "";

		public StringVar() {}

		public StringVar(System.String val) {
			m_val = val;
		}

		public override System.String ToString() {
			return m_val;
		}


		public override bool ToBool() {
			return m_val.Length > 0;
		}

		public override SingleVar add(SingleVar b) {
			return new StringVar(this.m_val + b.ToString());
		}

		public override bool MoreThan(SingleVar b) {
			if (!b.IsString())
				throw new CalcException("Неправильные операнды.");
			else {
				return b.ToString().CompareTo(m_val) < 0;
			}
		}

		public override System.Object Clone() {
			return new StringVar(m_val);
		}

		public override void Serialise(BinaryWriter bw) {
			bw.Write('s');
			bw.Write(m_val);
		}

		public override bool Equals(System.Object obj) {
			return (obj is StringVar) && obj.ToString().Equals(m_val);
		}
	}
}