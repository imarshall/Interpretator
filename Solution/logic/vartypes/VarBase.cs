using System;
using System.IO;

namespace interpr.logic.vartypes {
	public abstract class VarBase : ICloneable , IComputable {
		public bool IsArray() {
			return (this is ArrayVar);
		}

		public bool IsNum() {
			return (this is NumVar);
		}

		public bool IsString() {
			return (this is StringVar);
		}

		public bool IsInt() {
			return (this is IntVar);
		}

		public bool IsReal() {
			return (this is RealVar);
		}

		public bool IsSingle() {
			return (this is SingleVar);
		}


		public virtual VarBase Compute() {
			return this.Clone() as VarBase;
		}

		public abstract System.Object Clone();

		public override abstract string ToString();

		public abstract void Serialise(BinaryWriter bw);
	}
}