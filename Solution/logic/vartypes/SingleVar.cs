using System;

namespace interpr.logic.vartypes {
	public abstract class SingleVar : VarBase {
		public abstract bool ToBool();

		private static int DigitVal(char d) {
			switch (d) {
				case '0':
					return 0;

				case '1':
					return 1;

				case '2':
					return 2;

				case '3':
					return 3;

				case '4':
					return 4;

				case '5':
					return 5;

				case '6':
					return 6;

				case '7':
					return 7;

				case '8':
					return 8;

				case '9':
					return 9;

				default:
					throw new OtherException("Error in SingleVar.DigitVal()!");

			}
		}

		public static SingleVar FromString(System.String str) {
			try {
				int ind1;
				int len = str.Length;
				if (len == 0) {
					return null;
				}
				char[] str1 = str.ToCharArray();
				if (str1[0] == '\"') {
					if ((len < 2) || str1[len - 1] != '\"') {
						return null;
					}
					else {
						return new StringVar(new System.String(str1, 1, len - 2));
					}
				}
				bool nsign = (str1[0] == '-');
				if (nsign || (str1[0] == '+')) {
					ind1 = 1;
				}
				else {
					ind1 = 0;
				}
				if ((len <= ind1) || !System.Char.IsDigit(str1[ind1])) {
					return null;
				}
				int i_res = 0;
				for (; (ind1 < len) && System.Char.IsDigit(str1[ind1]); ind1++) {
					i_res *= 10;
					i_res += DigitVal(str1[ind1]);
				}
				if (ind1 == len) {
					if (nsign) {
						return new IntVar(- i_res);
					}
					else {
						return new IntVar(i_res);
					}
				}
				double fract = 0;
				int fractlen = 0;
				if (str1[ind1] == '.') {
					ind1++;
					for (; (ind1 < len) && System.Char.IsDigit(str1[ind1]); ind1++, fractlen++) {
						fract *= 10;
						fract += DigitVal(str1[ind1]);
					}
					for (; fractlen > 0; fractlen--) {
						fract /= 10;
					}
				}
				if (len == ind1) {
					if (nsign) {
						return new RealVar(- (i_res + fract));
					}
					else {
						return new RealVar(i_res + fract);
					}
				}
				else if ((str1[ind1] == 'e') || (str1[ind1] == 'E')) {
					ind1++;
					if (ind1 == len) {
						return null;
					}
					int exp = 0;
					bool expnsign = (str1[ind1] == '-');
					if ((str1[ind1] == '+') || expnsign) {
						ind1++;
						if (ind1 == len) {
							return null;
						}
					}
					for (; (ind1 < len) && System.Char.IsDigit(str1[ind1]); ind1++) {
						exp *= 10;
						exp += DigitVal(str1[ind1]);
					}
					if (ind1 != len) {
						return null;
					}
					else {
						double res = i_res + fract;
						if (expnsign) {
							for (; exp > 0; exp--) {
								res /= 10;
							}
						}
						else {
							for (; exp > 0; exp--) {
								res *= 10;
							}
						}
						if (nsign) {
							return new RealVar(- res);
						}
						else {
							return new RealVar(res);
						}
					}
				}
				else {
					return null;
				}
			}
			catch (Exception exc) {
				return null;
			}
		}

		public abstract SingleVar add(SingleVar b);

		public abstract override bool Equals(System.Object obj);

		public abstract bool MoreThan(SingleVar b);

		public IntVar eq(SingleVar b) {
			return new IntVar(this.Equals(b));
		}

		public IntVar ne(SingleVar b) {
			return new IntVar(!this.Equals(b));
		}

		public IntVar lt(SingleVar b) {
			return new IntVar(b.MoreThan(this));
		}

		public IntVar gt(SingleVar b) {
			return new IntVar(this.MoreThan(b));
		}

		public IntVar le(SingleVar b) {
			return new IntVar(!this.MoreThan(b));
		}

		public IntVar ge(SingleVar b) {
			return new IntVar(!b.MoreThan(this));
		}

		public IntVar and(SingleVar b) {
			return new IntVar(this.ToBool() && b.ToBool());
		}

		public IntVar or(SingleVar b) {
			return new IntVar(this.ToBool() || b.ToBool());
		}

		public IntVar not() {
			return new IntVar(!this.ToBool());
		}

		public IntVar xor(SingleVar b) {
			bool p = this.ToBool();
			bool q = b.ToBool();
			return new IntVar(((!p) && q) || (p && (!q)));
		}

		public SingleVar be(SingleVar b) {
			return new IntVar(this.ToBool() == b.ToBool());
		}
	}
}