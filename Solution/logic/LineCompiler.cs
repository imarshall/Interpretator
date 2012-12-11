using System;
using interpr.logic.operators;

namespace interpr.logic {
	public class LineCompiler {
		private LineCompiler() {}

		public static Command CompileCommand(string str) {
			Parser p = new Parser(str);
			if (!p.HasMore()) {
				return new EmptyCommand();
			}
			String pstr = p.GetString();
			int posa = pstr.IndexOf(":=");
			if (posa >= 0) {
				int cq = 0;
				for (int iq = 0; iq < posa; iq++)
					if (pstr[iq] == '\"')
						cq++;
				if (cq%2 == 0) {
					try {
						if (posa == 0)
							throw new SyntaxErrorException("Синтаксическая ошибка");
						try {
							if (pstr[posa - 1] == '}') {
								int posob = pstr.IndexOf('{');
								if ((posob < 0) || (posob > posa))
									throw new SyntaxErrorException("Синтаксическая ошибка");
								return new AssignCommand(pstr.Substring(0, posob),
								                         pstr.Substring(posob + 1, posa - posob - 2),
								                         pstr.Substring(posa + 2));
							} else {
								return new AssignCommand(pstr.Substring(0, posa),
								                         pstr.Substring(posa + 2));
							}
						} catch {
							throw new SyntaxErrorException("Синтаксическая ошибка");
						}
					} catch (CalcException ex) {
						throw new SyntaxErrorException(ex.Message);
					}
				}
			}
			p.MoveNext();
			string firsttoken = (p.Current as String);
			try {
				if (firsttoken == "clear") {
					if (!p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					Command cc = new ClearCommand(p.Current as String);
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return cc;
				}
				if (firsttoken == "print") {
					Expression expr = new Expression(p);
					return new PrintCommand(expr);
				} else if (firsttoken == "println") {
					Expression expr = new Expression(p);
					return new PrintLnCommand(expr);
				} else if (firsttoken == "call") {
					Expression expr = new Expression(p);
					return new CallCommand(expr);
				} else {
					p.Reset();
					Expression expr1 = new Expression(p);
					return new PrintLnCommand(expr1);
				}
			} catch (SyntaxErrorException ex) {
				throw ex;
			} catch (Exception ex) {
				throw new SyntaxErrorException(ex.Message);
			}

		}

		public static IOperator CompileOperator(string str) {
			Parser p = new Parser(str);
			if (!p.HasMore()) {
				return new EmptyCommand();
			}
			String pstr = p.GetString();
			p.MoveNext();
			string firsttoken = (p.Current as String);
			if (firsttoken == "for") {
				try {
					return ParseForStatement(p.GetString());
				} catch (SyntaxErrorException ex) {
					throw ex;
				} catch (Exception ex) {
					throw new SyntaxErrorException(ex.Message);
				}
			}
			int posa = pstr.IndexOf(":=");
			if (posa >= 0) {
				int cq = 0;
				for (int iq = 0; iq < posa; iq++)
					if (pstr[iq] == '\"')
						cq++;
				if (cq%2 == 0) {
					try {
						if (posa == 0)
							throw new SyntaxErrorException("Синтаксическая ошибка");
						try {
							if (pstr[posa - 1] == '}') {
								int posob = pstr.IndexOf('{');
								if ((posob < 0) || (posob > posa))
									throw new SyntaxErrorException("Синтаксическая ошибка");
								return new AssignCommand(pstr.Substring(0, posob),
								                         pstr.Substring(posob + 1, posa - posob - 2),
								                         pstr.Substring(posa + 2));
							} else {
								return new AssignCommand(pstr.Substring(0, posa),
								                         pstr.Substring(posa + 2));
							}
						} catch {
							throw new SyntaxErrorException("Синтаксическая ошибка");
						}
					} catch (CalcException ex) {
						throw new SyntaxErrorException(ex.Message);
					}
				}
			}
			try {
				if (firsttoken == "clear") {
					if (!p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					Command cc = new ClearCommand(p.Current as String);
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return cc;
				} else if (firsttoken == "next") {
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return new NextOperator();
				} else if (firsttoken == "else") {
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return new ElseOperator();
				} else if (firsttoken == "endif") {
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return new EndifOperator();
				} else if (firsttoken == "loop") {
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return new LoopOperator();
				} else if (firsttoken == "return") {
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return new ReturnOperator();
				} else if (firsttoken == "error") {
					if (p.MoveNext())
						throw new SyntaxErrorException("Синтаксическая ошибка");
					return new ErrorOperator();
				}
				Expression expr = new Expression(p);
				if (firsttoken == "print")
					return new PrintCommand(expr);
				else if (firsttoken == "println")
					return new PrintLnCommand(expr);
				else if (firsttoken == "call")
					return new CallCommand(expr);
				else if (firsttoken == "while")
					return new WhileOperator(expr);
				else if (firsttoken == "if")
					return new IfOperator(expr);
				else if (firsttoken == "elseif")
					return new ElseifOperator(expr);
				else
					throw new SyntaxErrorException("Синтаксическая ошибка");
			} catch (SyntaxErrorException ex) {
				throw ex;
			} catch (Exception ex) {
				throw new SyntaxErrorException(ex.Message);
			}
		}

		private static IOperator ParseForStatement(string str) {
			str = str.Substring(3);
			int assignpos = str.IndexOf(":=");
			if (assignpos < 0)
				throw new SyntaxErrorException("Неправильный синтаксис оператора for");
			string countername = str.Substring(0, assignpos).Trim();
			if (!Parser.IsID(countername))
				throw new SyntaxErrorException("Неправильный синтаксис оператора for");
			str = str.Substring(assignpos + 2);
			int colonpos = str.IndexOf(":");
			if (colonpos < 0)
				throw new SyntaxErrorException("Неправильный синтаксис оператора for");
			string expr1str = str.Substring(0, colonpos);
			string expr2str = str.Substring(colonpos + 1);
			Expression expr1 = new Expression(expr1str);
			Expression expr2 = new Expression(expr2str);
			return new ForOperator(countername, expr1, expr2);
		}

	}
}