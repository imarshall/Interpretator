using System;
using interpr.logic.vartypes;

namespace interpr.logic {
	public class Expression {
		public Expression(String str) {
			Parser p = new Parser(str);
			Analyse(p);
		}

		public Expression(Parser p) {
			Analyse(p);
		}

		private class Element {
			internal IComputable m_o;
			internal Element m_next;

			internal Element(IComputable obj, Element next) {
				m_o = obj;
				m_next = next;
			}
		}

		private Element m_top = null;
		private Element m_bottom = null;

		private int m_c = 0;

		private void AddFront(IComputable obj) {
			m_c++;
			if (m_c == 1)
				m_top = m_bottom = new Element(obj, null);
			else {
				Element t = new Element(obj, null);
				m_bottom.m_next = t;
				m_bottom = t;
			}
		}

		private void Analyse(Parser p) {
			try {
				LinkedList l = new LinkedList();
				while (p.MoveNext())
					l.Add(p.Current);
				OPZ(l);
			}
			catch (CalcException ex) {
				throw ex;
			}
			catch {
				throw new SyntaxErrorException("Синтаксическая ошибка в выражении");
			}
		}

		private void OPZ(LinkedList tokens) {
		/* ** Бинарная операция выталкивает из стека в результат
		 * все операции с большим или равным приоритетом, затем
		 * записывается в стек сама
		 * ** Унарная операция записывается в стек
         * ** Открывающая скобка записывается в стек
         * ** Закрывающая скобка выталкивает в результат все операции
         * из стека до открывающей скобки, затем
         * скобки уничтожаются и выталкиваются унарные операции
         * ** Переменная или константа сразу пишутся в результат, затем
         * выталкиваются из стека унарные операции
         * ** При вызове функции
         * сначала отдельно разбираются все операнды, затем в результат
         * дописывается сама функция, как операция
         * ** Обращение к элементу массива обрабатывается аналогично
         * В конце все оставшиеся в стеке операции выталкиваются в результат
         */
			InterprEnvironment env = InterprEnvironment.Instance;
			if (tokens.IsEmpty()) return;
			LinkedList.Iterator itr = tokens.GetIterator();
			LinkedList stk = new LinkedList();
			while (itr.HasMore) {
				string si = (itr.Step() as System.String);
				if (si == "(") {
					stk.Add(O_BR);
				}
				else if (si == ")") {
					while (true) {
						object o = stk.RemoveLast();
						if (o == O_BR) break;
						AddFront(new Call(o as Operation));
					}
					while ((!stk.IsEmpty()) && IsUnary(stk.Last)) {
						AddFront(new Call(stk.RemoveLast() as Operation));
					}
				}
				else if (Parser.IsID(si)) {
					bool bfun = false;
					bool barray = false;
					if (itr.HasMore) {
						string s = (itr.Step() as System.String);
						if (s == "[")
							bfun = true;
						else if (s == "{")
							barray = true;
						else
							itr.Previous();
					}
					if (bfun) {
						LinkedList l = null;
						while (true) {
							l = new LinkedList();
							int level = 0;
							while (true) {
								if (!itr.HasMore)
									throw new SyntaxErrorException("Синтаксическая ошибка в выражении");
								string sj = (itr.Step() as System.String);
								if (sj == "[") {
									level++;
									l.Add(sj);
								}
								else if (sj == "]") {
									if (level == 0)
										goto label1;
									else {
										level--;
										l.Add(sj);
									}
								}
								else if (sj == ",") {
									if (level > 0)
										l.Add(sj);
									else
										break;
								}
								else
									l.Add(sj);
							}
							OPZ(l);
						}
						label1:
						if (l != null)
							OPZ(l);
						Operation sub = env.GetFunction(si);
						AddFront(new Call(sub));
						while ((stk.Count > 0) && IsUnary(stk.Last)) {
							AddFront(new Call((Operation) stk.RemoveLast()));
						}
					}
					else if (barray) {
						LinkedList l = new LinkedList();
						int level = 0;
						while (true) {
							if (!itr.HasMore)
								throw new SyntaxErrorException("Синтаксическая ошибка в выражении");
							String sj = (String) itr.Step();
							if (sj == "{") {
								level++;
								l.Add(sj);
							}
							else if (sj == "}") {
								if (level == 0)
									break;
								else {
									level--;
									l.Add(sj);
								}
							}
							else
								l.Add(sj);
						}
						OPZ(l);
						VarName v = new VarName(si);
						AddFront(v);
						AddFront(new Call(Operation.INDEX));
						while ((stk.Count > 0) && IsUnary(stk.Last)) {
							AddFront(new Call(stk.RemoveLast() as Operation));
						}
					}
					else {
						VarName v = new VarName(si);
						AddFront(v);
						while ((stk.Count > 0) && IsUnary(stk.Last)) {
							AddFront(new Call(stk.RemoveLast() as Operation));
						}
					}
				}
				else {
					Operation op = StrToOperation(si);
					if (op == null) {
						SingleVar sv = SingleVar.FromString(si);
						if (si == null)
							throw new SyntaxErrorException("Синтаксическая ошибка в выражении");
						AddFront(sv);
						while ((stk.Count > 0) && IsUnary(stk.Last)) {
							AddFront(new Call(stk.RemoveLast() as Operation));
						}
					}
					else {
						//operation
						if (op == Operation.ADD) {
							itr.Previous();
							if (!itr.HasPrevious) {
								stk.Add(Operation.UPLUS);
								itr.Step();
								continue;
							}
							String strpr = (String) itr.Previous();
							itr.Step();
							itr.Step();
							if ((StrToOperation(strpr) != null) || (strpr == "(") ||
								(strpr == "[") || (strpr == "{")) {
								stk.Add(Operation.UPLUS);
								continue;
							}
						}
						else if (op == Operation.SUB) {
							itr.Previous();
							if (!itr.HasPrevious) {
								stk.Add(Operation.UMINUS);
								itr.Step();
								continue;
							}
							String strpr = (String) itr.Previous();
							itr.Step();
							itr.Step();
							if ((StrToOperation(strpr) != null) || (strpr == "(") ||
								(strpr == "[") || (strpr == "{")) {
								stk.Add(Operation.UMINUS);
								continue;
							}
						}
						else if (op == Operation.NOT) {
							stk.Add(op);
							continue; 
						}
						if (stk.IsEmpty() || (stk.Last == O_BR)) {
							stk.Add(op);
						}
						else {
							int pr = Priority(op);
							while (true) {
								if (stk.IsEmpty())
									break;
								Object stktop = stk.Last;
								if (stktop is Operation) {
									int pr1 = Priority(stktop as Operation);
									if ((pr <= pr1) && (pr1 < 6)) {
										AddFront(new Call(stktop as Operation));
										stk.RemoveLast();
									}
									else
										break;
								}
								else
									break;
							}
							stk.Add(op);
						}
					}
				}
			}
			while (!stk.IsEmpty()) {
				Object o = stk.RemoveLast();
				AddFront(new Call(o as Operation));
			}
		}


		public VarBase Calculate() {
			if (m_c == 0)
				throw new CalcException("Ошибка: пустое выражение.");
			Element top1 = null;
			Element cur = m_top;
			try {
				for (; cur != null; cur = cur.m_next) {
					if (cur.m_o is Call) {
						int rc = (cur.m_o as Call).ReqCount;
						ArgList al = new ArgList();
						for (int i = 0; i < rc; i++) {
							if (top1 == null)
								throw new CalcException("Ошибка при вычислении выражения");
							al.Add(top1.m_o.Compute());
							top1 = top1.m_next;
						}
						(cur.m_o as Call).SetArgList(al);
						top1 = new Element((cur.m_o as Call).Compute(), top1);
					}
					else {
						top1 = new Element(cur.m_o, top1);
					}
				}
				if ((top1 == null) || (top1.m_next != null))
					throw new CalcException("Ошибка при вычислении выражения");
				return top1.m_o.Compute();
			}
			catch (CalcException ex) {
				throw ex;
			}
			catch {
				throw new CalcException("Ошибка при вычислении выражения");
			}
		}

		private static Operation StrToOperation(String str) {
			//не возвращает унарные плюс и минус
			if (str == "+")
				return Operation.ADD;
			else if (str == "-")
				return Operation.SUB;
			else if (str == "*")
				return Operation.MUL;
			else if (str == "/")
				return Operation.DIV;
			else if (str == "~")
				return Operation.NOT;
			else if (str == "|")
				return Operation.OR;
			else if (str == "&")
				return Operation.AND;
			else if (str == "^")
				return Operation.XOR;
			else if (str == "~=")
				return Operation.BE;
			else if (str == "=")
				return Operation.EQ;
			else if (str == "<>")
				return Operation.NE;
			else if (str == ">=")
				return Operation.GE;
			else if (str == "<=")
				return Operation.LE;
			else if (str == ">")
				return Operation.GT;
			else if (str == "<")
				return Operation.LT;
			else
				return null;
		}

		private static int Priority(Operation op) {
			if ((op == Operation.OR) || (op == Operation.XOR) ||
				(op == Operation.BE))
				return 1;
			else if (op == Operation.AND)
				return 2;
			else if ((op == Operation.EQ) || (op == Operation.NE) ||
				(op == Operation.LE) || (op == Operation.LT) ||
				(op == Operation.GE) || (op == Operation.GT))
				return 3;
			else if ((op == Operation.ADD) || (op == Operation.SUB))
				return 4;
			else if ((op == Operation.MUL) || (op == Operation.DIV))
				return 5;
			else
				return 6;
		}

		private static bool IsBinary(Operation op) {
			return Priority(op) < 6;
		}

		private static bool IsUnary(object obj) {
			return ((obj == Operation.NOT) || (obj == Operation.UPLUS) ||
				(obj == Operation.UMINUS));
		}

		private class BR_c {}

		private static object O_BR = new BR_c();

	}

}