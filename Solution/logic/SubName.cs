using System;
using interpr.logic.vartypes;

namespace interpr.logic
{
	public class SubName : Operation
	{
		string m_name;

		public SubName(string name)
		{
			m_name = name;
		}

		public override int ReqCount {
			get {
				return InterprEnvironment.Instance.GetSub(m_name).ReqCount;
			}
		}

		public override VarBase Perform(ArgList al) {
			return InterprEnvironment.Instance.GetSub(m_name).Perform(al);
		}
	}
}
