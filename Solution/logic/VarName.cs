using System;
using interpr.logic.vartypes;

namespace interpr.logic {
	public class VarName : IComputable {
		private string m_name;

		public VarName(string name) {
			if (!Parser.IsID(name))
				throw new SyntaxErrorException(name + " - не является корректным идентификатором");
			m_name = name;
		}

		public VarBase Compute() {
			Namespace ns = InterprEnvironment.Instance.CurrentNamespace;
			VarBase var = ns.Get(m_name);
			if (var == null)
				throw new CalcException("Неинициализированная переменная");
			return var.Clone() as VarBase;
		}
	}
}