namespace interpr.logic {
	public class LinkedList {
		private Link m_bottom;
		private Link m_top;
		private int m_count;

		public LinkedList() {
			m_bottom = m_top = null;
			m_count = 0;
		}

		public void Add(object obj) {
			m_count++;
			if (m_top == null) {
				m_top = new Link();
				m_top.next = m_top.prev = null;
				m_top.val = obj;
				m_bottom = m_top;
			} else {
				Link l = new Link();
				l.next = null;
				l.prev = m_top;
				l.val = obj;
				m_top.next = l;
				m_top = l;
			}
		}

		public void AddLast(object obj) {
			Add(obj);
		}

		public void AddFirst(object obj) {
			m_count++;
			if (m_top == null) {
				m_top = new Link();
				m_top.next = m_top.prev = null;
				m_top.val = obj;
				m_bottom = m_top;
			} else {
				Link l = new Link();
				l.prev = null;
				l.next = m_bottom;
				l.val = obj;
				m_bottom.prev = l;
				m_bottom = l;
			}
		}

		public int Count {
			get { return m_count; }
		}

		public bool IsEmpty() {
			return m_count == 0;
		}

		public object Last {
			get {
				if (m_top == null)
					throw new LinkedListException("Read from empty list");
				return m_top.val;
			}
		}

		public object First {
			get {
				if (m_bottom == null)
					throw new LinkedListException("Read from empty list");
				return m_bottom.val;
			}
		}

		public object RemoveLast() {
			if (m_top == null)
				throw new LinkedListException("Read from empty list");
			object res = m_top.val;
			m_top = m_top.prev;
			if (m_top == null)
				m_bottom = null;
			else
				m_top.next = null;
			m_count--;
			return res;
		}

		public object RemoveFirst() {
			if (m_bottom == null)
				throw new LinkedListException("Read from empty list");
			object res = m_bottom.val;
			m_bottom = m_bottom.next;
			if (m_bottom == null)
				m_top = null;
			else
				m_bottom.prev = null;
			m_count--;
			return res;
		}

		public Iterator GetIterator() {
			return new Iterator(this, 0);
		}

		public Iterator GetIterator(int index) {
			return new Iterator(this, index);
		}

		private class Link {
			internal Link prev, next;
			internal object val;
		}

		public class Iterator {
			private LinkedList m_list;
			private Link m_link;

			internal Iterator(LinkedList list, int index) {
				m_list = list;
				if (m_list.IsEmpty()) {
					m_link = null;
				}
				if (index > m_list.Count)
					throw new LinkedListException("Index out of list size");
				if (index == m_list.Count) {
					m_link = null;
				} else {
					m_link = m_list.m_bottom;
					while ((index--) > 0) {
						m_link = m_link.next;
					}
				}
			}

			public bool HasPrevious {
				get {
					if (m_link == null)
						return m_list.m_top != null;
					else
						return m_link.prev != null;
				}
			}

			public object Previous() {
				if (m_link == null) {
					if (m_list.m_top == null)
						throw new LinkedListException("Read from empty list");
					m_link = m_list.m_top;
					return m_link.val;
				} else {
					m_link = m_link.prev;
					if (m_link == null) {
						m_link = m_list.m_bottom;
						throw new LinkedListException("Read beyond then begin of the list");
					}
					return m_link.val;
				}
			}

			public bool HasMore {
				get { return m_link != null; }
			}

			public object Step() {
				if (m_link == null)
					throw new LinkedListException("Read beyond the end of the list");
				object res = m_link.val;
				m_link = m_link.next;
				return res;
			}

		}
	}

	public class LinkedListException : OtherException {
		public LinkedListException(string msg) : base(msg) {}
	}
}