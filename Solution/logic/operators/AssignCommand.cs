using System;
using interpr.logic;
using interpr.logic.vartypes;

namespace interpr.logic.operators {
	public class AssignCommand : Command {
		private string m_name;
		private Expression m_index;
		private bool m_array;
		private Expression m_expr;

		public AssignCommand(string name, string index, string expr) {
			m_array = true;
			m_expr = new Expression(expr);
			m_index = new Expression(index);
			m_name = name;
			if (!Parser.IsID(name))
				throw new SyntaxErrorException("��������� ������������� ��������� �����");
		}

		public AssignCommand(string name, string expr) {
			m_array = false;
			m_expr = new Expression(expr);
			m_name = name;
			if (!Parser.IsID(name))
				throw new SyntaxErrorException("��������� ������������� ��������� �����");
		}

		public override void Execute() {
			InterprEnvironment env = InterprEnvironment.Instance;
			if (m_array) {
				VarBase res = m_expr.Calculate();
				VarBase index = m_index.Calculate();
				if (!(index is IntVar))
					throw new CalcException("������ ������ ���� �����");
				if (!(res is SingleVar))
					throw new CalcException("������� ������� ��� �� ����� ���� ��������!");
				env.CurrentNamespace.AssignToElement(res as SingleVar, m_name, (index as IntVar).Val);
			}
			else
				env.CurrentNamespace[m_name] = m_expr.Calculate();
		}
	}
}