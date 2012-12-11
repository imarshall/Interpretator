using interpr.logic.vartypes;

namespace interpr.logic {
	public class Call : IComputable {
		private Operation m_op;
		private ArgList m_al = null;

		public Call(Operation op) {
			m_op = op;
		}

		public void SetArgList(ArgList al) {
			m_al = al;
		}

		public int ReqCount {
			get { return m_op.ReqCount; }
		}

		public VarBase Compute() {
			return m_op.Perform(m_al);
		}
	}
}
