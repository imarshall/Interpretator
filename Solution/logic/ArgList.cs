using interpr.logic.vartypes;

namespace interpr.logic {
	public class ArgList {
		private bool m_read = false;
		private LinkedList m_list = new LinkedList();
		private LinkedList.Iterator m_i = null;

		public void Add(VarBase var) {
			if (m_read)
				throw new OtherException("Write to the argument list after reading begin");
			m_list.Add(var);
		}

		public VarBase Get() {
			if (!m_read)
				throw new OtherException("Try to read from argument list before reset");
			if (!m_i.HasPrevious)
				throw new OtherException("Try to read from empty argument list");
			m_read = true;
			IComputable obj = (m_i.Previous() as IComputable);
			if (obj == null)
				throw new CalcException("Переменная не инициализированна.");
			return obj.Compute();
		}

		public void Reset() {
			m_read = true;
			m_i = m_list.GetIterator(m_list.Count);
		}

		public int Count {
			get { return m_list.Count; }
		}
	}
}
