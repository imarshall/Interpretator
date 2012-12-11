using System;
using interpr.logic.vartypes;

namespace interpr.logic {
	public abstract class Operation {
		public abstract int ReqCount { get; }

		public abstract VarBase Perform(ArgList al);

		public static readonly Operation ABS = new ABS_c();

		private class ABS_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (arg1 is IntVar)
					return ((arg1 as IntVar).Val>0) ? (arg1.Clone() as IntVar) : (new IntVar(-((IntVar) arg1).Val));
				else if (arg1 is RealVar)
					return ((arg1 as RealVar).Val>0) ? (arg1.Clone() as RealVar) : (new RealVar(-((RealVar) arg1).Val));
				else
					throw new CalcException("Неправильные аргументы функции");
			}
		}

		public static readonly Operation ADD = new ADD_c();

		private class ADD_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).add(arg2 as SingleVar);
			}
		}

		public static readonly Operation AND = new AND_c();

		private class AND_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).and(arg2 as SingleVar);
			}
		}

		public static readonly Operation ARCCOS = new ARCCOS_c();

		private class ARCCOS_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Acos((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation ARCSIN = new ARCSIN_c();

		private class ARCSIN_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Asin((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}


		public static readonly Operation ARCTG = new ARCTG_c();

		private class ARCTG_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Atan((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation BE = new BE_c();

		private class BE_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).be(arg2 as SingleVar);
			}
		}

		public static readonly Operation COS = new COS_c();

		private class COS_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Cos((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation DEFINED = new DEFINED_c();

		private class DEFINED_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if(al.Count!=ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!((arg1.IsArray()&&arg2.IsInt()))) {
					throw new CalcException("Неверные типы операндов");
				}
				return (arg1 as ArrayVar).IsElementDefined((arg2 as IntVar).Val);
			}
		}

		public static readonly Operation DIV = new DIV_c();

		private class DIV_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsNum()&&arg2.IsNum()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as NumVar).div(arg2 as NumVar);
			}
		}

		public static readonly Operation EQ = new EQ_c();

		private class EQ_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).eq(arg2 as SingleVar);
			}
		}

		public static readonly Operation EXP = new EXP_c();

		private class EXP_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Exp((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation GE = new GE_c();

		private class GE_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).ge(arg2 as SingleVar);
			}
		}

		public static readonly Operation GT = new GT_c();

		private class GT_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).gt(arg2 as SingleVar);
			}
		}

		public static readonly Operation IDIV = new IDIV_c();

		private class IDIV_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsInt()&&arg2.IsInt()))
					throw new CalcException("Неверные типы операндов");
				try {
					int res = (arg1 as IntVar).Val / (arg2 as IntVar).Val;
					return new IntVar(res);
				}
				catch(System.ArithmeticException ex) {
					throw new CalcException("Ошибка в вычислениях");	
				}
			}
		}

		public static readonly Operation IFF = new IFF_c();

		private class IFF_c : Operation {
			public override int ReqCount {
				get { return 3; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				VarBase arg3 = al.Get();
				if(!arg1.IsSingle())
					throw new CalcException("Значение условия не может быть массивом");
				if((arg1 as SingleVar).ToBool())
					return (arg2.Clone() as VarBase);
				else
					return (arg3.Clone() as VarBase);
			}
		}


		public static readonly Operation IMOD = new IMOD_c();

		private class IMOD_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsInt()&&arg2.IsInt()))
					throw new CalcException("Неверные типы операндов");
				try {
					int res = (arg1 as IntVar).Val % (arg2 as IntVar).Val;
					return new IntVar(res);
				}
				catch(System.ArithmeticException ex) {
					throw new CalcException("Ошибка в вычислениях");	
				}
			}
		}

		public static readonly Operation INDEX = new INDEX_c();

		private class INDEX_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg2.IsArray()&&arg1.IsInt()))
					throw new CalcException("Неверные типы операндов");
				return (arg2 as ArrayVar)[(arg1 as IntVar).Val];
			}
		}

		public static readonly Operation ISARRAY = new ISARRAY_c();

		private class ISARRAY_c:Operation {
			public override int ReqCount {
				get {return 1;}
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				return new IntVar(al.Get().IsArray());
			}

		}

		public static readonly Operation ISINT = new ISINT_c();

		private class ISINT_c:Operation {
			public override int ReqCount {
				get {return 1;}
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				return new IntVar(al.Get().IsInt());
			}
		}

		public static readonly Operation ISNUM = new ISNUM_c();

		private class ISNUM_c:Operation {
			public override int ReqCount {
				get {return 1;}
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				return new IntVar(al.Get().IsNum());
			}
		}

		public static readonly Operation ISREAL = new ISREAL_c();

		private class ISREAL_c:Operation {
			public override int ReqCount {
				get {return 1;}
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				return new IntVar(al.Get().IsReal());
			}
		}
		public static readonly Operation ISSINGLE = new ISSINGLE_c();

		private class ISSINGLE_c:Operation {
			public override int ReqCount {
				get {return 1;}
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				return new IntVar(al.Get().IsSingle());
			}
		}
		public static readonly Operation ISSTRING = new ISSTRING_c();

		private class ISSTRING_c:Operation {
			public override int ReqCount {
				get {return 1;}
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				return new IntVar(al.Get().IsString());
			}
		}

		public static readonly Operation LE = new LE_c();

		private class LE_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).le(arg2 as SingleVar);
			}
		}

		public static readonly Operation LG = new LG_c();

		private class LG_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Log10((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation LN = new LN_c();

		private class LN_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Log((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation LOG = new LOG_c();

		private class LOG_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if (!(arg1.IsNum()&&arg2.IsNum()))
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(
						Math.Log(
						(arg1 as NumVar).ToDouble(),
						(arg2 as NumVar).ToDouble()
						)
						);
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation LT = new LT_c();

		private class LT_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).lt(arg2 as SingleVar);
			}
		}

		public static readonly Operation MUL = new MUL_c();

		private class MUL_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsNum()&&arg2.IsNum()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as NumVar).mul(arg2 as NumVar);
			}
		}

		public static readonly Operation NE = new NE_c();

		private class NE_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).ne(arg2 as SingleVar);
			}
		}

		public static readonly Operation NOT = new NOT_c();

		private class NOT_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if(!arg1.IsSingle())
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).not();
			}
		}

		public static readonly Operation OR = new OR_c();

		private class OR_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).or(arg2 as SingleVar);
			}
		}
		public static readonly Operation PI = new PI_c();

		private class PI_c : Operation {
			public override int ReqCount {
				get { return 0; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				return new RealVar(Math.PI);
			}
		}

		public static readonly Operation POW = new POW_c();

		private class POW_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if (!(arg1.IsNum()&&arg2.IsNum()))
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(
						Math.Pow(
						(arg1 as NumVar).ToDouble(),
						(arg2 as NumVar).ToDouble()
						)
						);
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation RANDOM = new RANDOM_c();

		private class RANDOM_c : Operation {
			private System.Random m_random = new System.Random();

			public override int ReqCount {
				get { return 0; }
			}

			public override VarBase Perform(ArgList al) {
				if(al.Count!=ReqCount)
					throw new OtherException("Invalid argument list");
				return new RealVar(m_random.NextDouble());
			}

		}
		
		public static readonly Operation SIN = new SIN_c();

		private class SIN_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Sin((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation SIZE = new SIZE_c();

		private class SIZE_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsArray())
					throw new CalcException("Неправильные аргументы функции");
				return new IntVar((arg1 as ArrayVar).GetSize());
			}
		}


		public static readonly Operation SQRT = new SQRT_c();

		private class SQRT_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Sqrt((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation STRLEN = new STRLEN_c();

		private class STRLEN_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsString())
					throw new CalcException("Неправильные аргументы функции");
				return new IntVar((arg1 as StringVar).Val.Length);
			}
		}

		public static readonly Operation STRPOS = new STRPOS_c();

		private class STRPOS_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if (!(arg1.IsString()&&arg2.IsString()))
					throw new CalcException("Неправильные аргументы функции");
				int res = (arg1 as StringVar).Val.IndexOf((arg2 as StringVar).Val);
				return new IntVar(res);
			}
		}

		public static readonly Operation SUB = new SUB_c();

		private class SUB_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsNum()&&arg2.IsNum()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as NumVar).sub(arg2 as NumVar);
			}
		}

		public static readonly Operation SUBSTR = new SUBSTR_c();

		private class SUBSTR_c : Operation {
			public override int ReqCount {
				get { return 3; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				VarBase arg3 = al.Get();
				if(!(arg1.IsString()&&arg2.IsInt()&&arg3.IsInt()))
					throw new CalcException("Неверные типы операндов");
				string str = (arg1 as StringVar).Val;
				int b = (arg2 as IntVar).Val;
				int l = (arg3 as IntVar).Val;
				if ((b < 0) || (l < 0))
					throw new CalcException("Неправильные аргументы функции");
				int sl = str.Length;
				if (b >= sl)
					return new StringVar("");
				if (sl - b < l)
					l = sl - b;
				return new StringVar(str.Substring(b, l));
			}
		}

		public static readonly Operation TG = new TG_c();

		private class TG_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (!arg1.IsNum())
					throw new CalcException("Неправильные аргументы функции");
				try {
					return new RealVar(Math.Tan((arg1 as NumVar).ToDouble()));
				} 
				catch {
					throw new CalcException("Ошибка в вычислениях");
				}
			}
		}

		public static readonly Operation TOINT = new TOINT_c();

		private class TOINT_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (arg1.IsArray())
					throw new CalcException("Ошибка преобразования");
				if(arg1.IsString()) {
					try {
						int i = System.Convert.ToInt32((arg1 as StringVar).Val);
						return new IntVar(i);
					}
					catch {
						throw new CalcException("Ошибка преобразования");
					}
				}
				else if (arg1.IsReal()) {
					return new IntVar(System.Convert.ToInt32((arg1 as RealVar).Val));
				}
				else if (arg1.IsInt()) {
					return (arg1.Clone() as VarBase);
				}
				else
					return null;
			}
		}

		public static readonly Operation TOREAL = new TOREAL_c();
		
		private class TOREAL_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if (arg1.IsArray())
					throw new CalcException("Ошибка преобразования");
				if(arg1.IsString()) {
					try {
						double d = System.Convert.ToDouble((arg1 as StringVar).Val);
						return new RealVar(d);
					}
					catch {
						throw new CalcException("Ошибка преобразования");
					}
				}
				else if (arg1.IsNum()) {
					return new RealVar((arg1 as NumVar).ToDouble());
				}
				else
					return null;
			}
		}

		public static readonly Operation TOSTRING = new TOSTRING_c();

		private class TOSTRING_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				return new StringVar(arg1.ToString());
			}
		}

		public static readonly Operation UMINUS = new UMINUS_c();

		private class UMINUS_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if(!(arg1 is NumVar))
					throw new CalcException("Неверные типы операндов");
				else if(arg1 is IntVar)
					return new IntVar(-(arg1 as IntVar).Val);
				else
					return new RealVar(-(arg1 as NumVar).ToDouble());
			}
		}

		
		public static readonly Operation UPLUS = new UPLUS_c();

		private class UPLUS_c : Operation {
			public override int ReqCount {
				get { return 1; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				if(!(arg1 is NumVar))
					throw new CalcException("Неверные типы операндов");
				return (arg1.Clone() as VarBase);
			}
		}

		public static readonly Operation XOR = new XOR_c();

		private class XOR_c : Operation {
			public override int ReqCount {
				get { return 2; }
			}

			public override VarBase Perform(ArgList al) {
				if (al.Count != ReqCount)
					throw new OtherException("Invalid argument list");
				al.Reset();
				VarBase arg1 = al.Get();
				VarBase arg2 = al.Get();
				if(!(arg1.IsSingle()&&arg2.IsSingle()))
					throw new CalcException("Неверные типы операндов");
				return (arg1 as SingleVar).xor(arg2 as SingleVar);
			}
		}

	}
}
