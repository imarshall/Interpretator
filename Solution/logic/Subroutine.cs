using System;
using System.Collections;
using System.Threading;
using interpr.logic.vartypes;
using interpr.logic.operators;

namespace interpr.logic {
	public sealed class Subroutine  {
		private void AnalyseHeader(string str) {
			Parser header_p = new Parser(str);
			if (!header_p.MoveNext())
				throw new SyntaxErrorException("������ � ��������� �������");
			if ((header_p.Current as System.String) != m_name)
				throw new SyntaxErrorException("��� ������� �� ��������� � ������ �����");
			if ((!header_p.MoveNext()) || ((header_p.Current as String) != "["))
				throw new SyntaxErrorException("������ � ��������� �������");
			if ((!header_p.MoveNext()))
				throw new SyntaxErrorException("������ � ��������� �������");
			if ((header_p.Current as System.String != "]")) {
				string readstr;
				while (true) {
					readstr = (header_p.Current as System.String);
					if (!Parser.IsID(readstr))
						throw new SyntaxErrorException("������ � ��������� �������");
					m_args.Add(readstr);
					if (!header_p.MoveNext())
						throw new SyntaxErrorException("������ � ��������� �������");
					readstr = (header_p.Current as System.String);
					if (readstr == ",") {
						if (!header_p.MoveNext())
							throw new SyntaxErrorException("������ � ��������� �������");
					}
					else if (readstr == "]")
						break;
					else
						throw new SyntaxErrorException("������ � ��������� �������");
				}
			}
			if (header_p.MoveNext())
				throw new SyntaxErrorException("������ � ��������� �������");
			if (m_args.IndexOf("result") >= 0)
				throw new SyntaxErrorException("�������� ������� �� ����� ����� ��� \"result\"");
		}

		public Subroutine(string[] code, string name) {
			m_name = name;
			if (code.Length == 0)
				throw new SyntaxErrorException("���� ������� ����");
			AnalyseHeader(code[0]);
			int clen = code.Length;
			int i = 0;
			try {
				Stack stk = new Stack();
				m_operators.Add(new EmptyCommand()); //����� ���������� ���������� � �������
				for (i = 1; i < clen; i++) {
					IOperator op = LineCompiler.CompileOperator(code[i]);
					if (op == null)
						throw new SyntaxErrorException("�������������� ������");
					m_operators.Add(op);
					switch (op.GetKind()) {
						case OperatorKind.If:
						case OperatorKind.While:
						case OperatorKind.For:
							{
								stk.Push(i);
								break;
							}
						case OperatorKind.Elseif:
							{
								if (stk.Count == 0)
									throw new SyntaxErrorException("������ elseif");
								int j = (int) stk.Pop();
								switch ((m_operators[j] as IOperator).GetKind()) {
									case OperatorKind.If:
										{
											(m_operators[j] as IfOperator).NextPos = i;
											break;
										}
									case OperatorKind.Elseif:
										{
											(m_operators[j] as ElseifOperator).NextPos = i;
											break;
										}
									default:
										throw new SyntaxErrorException("������ elseif");
								}
								stk.Push(i);
								break;
							}
						case OperatorKind.Else:
							{
								if (stk.Count == 0)
									throw new SyntaxErrorException("������ else");
								int j = (int) stk.Pop();
								stk.Push(i);
								switch ((m_operators[j] as IOperator).GetKind()) {
									case OperatorKind.If:
										{
											(m_operators[j] as IfOperator).NextPos = i;
											break;
										}
									case OperatorKind.Elseif:
										{
											(m_operators[j] as ElseifOperator).NextPos = i;
											break;
										}
									default:
										throw new SyntaxErrorException("������ else");
								}
								break;
							}
						case OperatorKind.Endif:
							{
								if (stk.Count == 0)
									throw new SyntaxErrorException("������ endif");
								int j = (int) stk.Pop();
								switch ((m_operators[j] as IOperator).GetKind()) {
									case OperatorKind.If:
										{
											(m_operators[j] as IfOperator).NextPos = i;
											break;
										}
									case OperatorKind.Elseif:
										{
											(m_operators[j] as ElseifOperator).NextPos = i;
											break;
										}
									case OperatorKind.Else:
										{
											(m_operators[j] as ElseOperator).NextPos = i;
											break;
										}
									default:
										throw new SyntaxErrorException("������ endif");
								}
								break;
							}
						case OperatorKind.Loop:
							{
								if (stk.Count == 0)
									throw new SyntaxErrorException("������ loop");
								int j = (int) stk.Pop();
								if ((m_operators[j] as IOperator).GetKind() != OperatorKind.While)
									throw new SyntaxErrorException("������ loop");
								(m_operators[i] as LoopOperator).WhilePos = j;
								(m_operators[j] as WhileOperator).LoopPos = i;
								break;
							}
						case OperatorKind.Next:
							{
								if (stk.Count == 0)
									throw new SyntaxErrorException("������ next");
								int j = (int) stk.Pop();
								if ((m_operators[j] as IOperator).GetKind() != OperatorKind.For)
									throw new SyntaxErrorException("������ next");
								(m_operators[i] as NextOperator).ForPos = j;
								(m_operators[i] as NextOperator).ForOp = (m_operators[j] as ForOperator);
								(m_operators[j] as ForOperator).NextPos = i;
								break;
							}
					}
				}
				if (stk.Count != 0)
					throw new SyntaxErrorException("�� �������� ����");
			}
			catch (SyntaxErrorException ex) {
				throw new LineSyntaxException(ex.Message, m_name, i + 1);
			}
			m_count = m_operators.Count;
		}


		private string m_name;
		private ArrayList m_args = new ArrayList();
		private ArrayList m_operators = new ArrayList();
		private int m_count;

		public int ReqCount {
			get { return m_args.Count; }
		}

		public VarBase Perform(ArgList al) {
			Namespace ns = new Namespace(InterprEnvironment.Instance.CurrentNamespace);
			ns["result"] = new IntVar(0);
			int argc = m_args.Count;
			if (al.Count != argc)
				throw new CalcException("�������� ����� ����������");
			al.Reset();
			for (int i = 0; i < argc; i++) {
				ns[m_args[i] as System.String] = al.Get();
			}
			InterprEnvironment.Instance.CurrentNamespace = ns;
			Moment moment = new Moment(this);
			if (m_count > 1) {
				try {
					moment.Run();
				}
				catch (SyntaxErrorException ex) {
					throw ex;
				}
				catch (CalcException ex) {
					throw new CalcException("������ � ������� " + m_name + "[] � ������ " + (moment.Pos + 1) + " : " + ex.Message);
				}
			}
			VarBase res = ns["result"];
			InterprEnvironment.Instance.CurrentNamespace = ns.PreviousNamespace;
			if (res == null)
				throw new CalcException("������ � ������� " + m_name + "[] : ���������� result �� ���������� �� ������ ������");
			return res;
		}

		public class Moment {
			private Subroutine m_sub;
			private int m_pos;
			private static int s_break = 0;

			public static void Break() {
				Interlocked.Exchange(ref s_break, 1);
			}

			public int Pos {
				get { return m_pos; }
			}

			public Moment(Subroutine sub) {
				m_sub = sub;
				m_pos = 1;
				s_break = 0;
			}

			public void GoTo(int to) {
				m_pos = to;
			}

			public void Next() {
				m_pos++;
			}

			public void Run() {
				while (m_pos < m_sub.m_count) {
					if (s_break == 1)
						throw new CalcException("�������� �������������");
					(m_sub.m_operators[m_pos] as IOperator).Execute(this);
				}
			}

			public void Return() {
				m_pos = m_sub.m_count;
			}

			public IOperator Current {
				get { return m_sub.m_operators[m_pos] as IOperator; }
			}
		}
	}
}