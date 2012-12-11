using System.Collections;
using System.IO;

namespace interpr.logic.vartypes {
	public class ArrayVar : VarBase {
		public virtual IntVar Size {
			get { return new IntVar(m_list.Count); }
		}

		private ArrayList m_list;

		public ArrayVar() {
			m_list = new ArrayList();
		}

		public int GetSize() {
			return m_list.Count;
		}

		public void setAt(int index, SingleVar var) {
			if (var == null) {
				throw new CalcException("Ошибка");
			}
			if (index < 0)
				throw new CalcException("Индекс не может быть отрицательным");
			for (int ind = index, s = m_list.Count; ind >= s; ind--)
				m_list.Add(null);
			m_list[index] = var.Clone();
		}

		public SingleVar getAt(int index) {
			if (index < 0)
				throw new CalcException("Индекс не может быть отрицательным");
			if (index >= m_list.Count)
				throw new CalcException("Выход за пределы массива");
			else
				return (SingleVar) m_list[index];
		}

		public SingleVar this[int index] {
			get { return getAt(index); }
			set { setAt(index, value); }
		}

		public IntVar IsElementDefined(int index) {
			bool result = index>=0;
			result = result&&(index<m_list.Count);
			result = result&&(m_list[index]!=null);
			return new IntVar(result);
		}

		public override System.Object Clone() {
			ArrayVar res = new ArrayVar();
			int li = 0;
			SingleVar e = null;
			while (li < m_list.Count) {
				e = (SingleVar) m_list[li++];
				if (e != null)
					res.m_list.Add(e.Clone());
				else
					res.m_list.Add(null);
			}
			return res;
		}

		public override void Serialise(BinaryWriter bw) {
			bw.Write('a');
			int size = m_list.Count;
			bw.Write(size);
			for (int i = 0; i < size; i++) {
				if (m_list[i] == null)
					bw.Write('n');
				else
					(m_list[i] as VarBase).Serialise(bw);
			}
		}

		public override System.String ToString() {
			System.String res = "[";
			int li = 0;
			SingleVar e = null;
			if (li < m_list.Count) {
				e = (SingleVar) m_list[li++];
				if (e != null) {
					res += e.ToString();
				}
				else
					res += "-";
			}
			while (li < m_list.Count) {
				e = (SingleVar) m_list[li++];
				if (e != null) {
					res += ", " + e.ToString();
				}
				else
					res += ", -";
			}
			return res + "]";
		}
	}
}