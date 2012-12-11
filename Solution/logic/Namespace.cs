using System;
using System.Collections;
using interpr.logic.vartypes;

namespace interpr.logic {
	public class NamespaceSerializationException : Exception {
		public NamespaceSerializationException() : base() {}
	}

	public class Namespace {
		protected class Pair {
			internal string m_str;
			internal VarBase m_var;
		}

		protected ArrayList m_list = new ArrayList();
		protected int m_n = 0;

		private Namespace m_previous_namespace = null;

		public Namespace PreviousNamespace {
			get { return m_previous_namespace; }
		}

		public Namespace(Namespace previous) {
			m_previous_namespace = previous;
		}

		protected Namespace() {}

		public VarBase Get(string name) {
			if (m_n == 0)
				return null;
			int i = 0;
			Pair p;

			do {
				p = (m_list[i++] as Pair);
				if (p.m_str == name)
					return p.m_var;
			} while (i < m_n);
			return null;
		}

		public void Assign(VarBase var, string name) {
			Pair p;
			if (m_n != 0) {
				int i = 0;
				do {
					p = (m_list[i++] as Pair);
					if (p.m_str == name) {
						p.m_var = var;
						return;
					}
				} while (i < m_n);
			}
			p = new Pair();
			p.m_var = var;
			p.m_str = name;
			m_list.Add(p);
			m_n++;
		}

		public void AssignToElement(SingleVar var, string name, int index) {
			Pair p;
			if (m_n != 0) {
				int i = 0;
				do {
					p = (m_list[i++] as Pair);
					if (p.m_str == name) {
						if (!p.m_var.IsArray())
							throw new CalcException("Переменная не является массивом");
						(p.m_var as ArrayVar)[index] = var;
						return;
					}
				} while (i < m_n);
			}
			p = new Pair();
			p.m_var = new ArrayVar();
			(p.m_var as ArrayVar)[index] = var;
			p.m_str = name;
			m_list.Add(p);
			m_n++;
		}

		public void Remove(String name) {
			if (m_n == 0)
				return;
			int i = 0;
			do {
				Pair p = (m_list[i++] as Pair);
				if (p.m_str == name) {
					m_list.RemoveAt(i - 1);
					m_n--;
					return;
				}
			} while (i < m_n);
		}

		public VarBase this[string name] {
			set { Assign(value, name); }
			get { return Get(name); }
		}

	}
}