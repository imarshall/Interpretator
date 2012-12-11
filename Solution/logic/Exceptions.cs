using System;

namespace interpr.logic {
	public class CalcException : Exception {
		public CalcException(string msg) : base(msg) {}

		public CalcException() : base("") {}
	}

	public class SyntaxErrorException : CalcException {
		public SyntaxErrorException(string msg) : base(msg) {}

		public SyntaxErrorException() : base("") {}
	}

	public class LineSyntaxException : SyntaxErrorException {
		private int m_line;
		private string m_fun;

		public LineSyntaxException(string msg, string fun, int line) : base(msg) {
			m_line = line;
			m_fun = fun;
		}

		public int Line {
			get { return m_line; }
		}

		public string Function {
			get { return m_fun; }
		}
	}

	public class OtherException : Exception 
	{
		public OtherException(string msg) : base(msg) {}
	}

}