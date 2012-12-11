using System;
using System.IO;
using System.Collections;
using interpr.logic.vartypes;

namespace interpr.logic {
	public class ConsoleNamespace : Namespace {
		public struct VariableReport {
			public string name;
			public string val;
		}

		private const string FILE_NAME = "variables";

		public void Save() {
			FileStream fs = new FileStream(FILE_NAME, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);
			try {
				bw.Write(m_n);
				for (int i = 0; i < m_n; i++) {
					bw.Write((m_list[i] as Pair).m_str);
					(m_list[i] as Pair).m_var.Serialise(bw);
				}
			}
			finally {
				bw.Close();
			}
		}

		public void Restore() {
			FileInfo fi = new FileInfo(FILE_NAME);
			m_list = new ArrayList();
			m_n = 0;
			if (!fi.Exists)
				return;
			BinaryReader br = new BinaryReader(fi.Open(FileMode.Open));
			try {
				int count = br.ReadInt32();
				for (int i = 0; i < count; i++) {
					Pair p = new Pair();
					p.m_str = br.ReadString();
					char vt = br.ReadChar();
					switch (vt) {
						case 's':
							{
								p.m_var = new StringVar(br.ReadString());
								break;
							}
						case 'i':
							{
								p.m_var = new IntVar(br.ReadInt32());
								break;
							}
						case 'r':
							{
								p.m_var = new RealVar(br.ReadDouble());
								break;
							}
						case 'a':
							{
								int elcount = br.ReadInt32();
								ArrayVar av = new ArrayVar();
								for (int j = 0; j < elcount; j++) {
									char elt = br.ReadChar();
									switch (elt) {
										case 'n':
											{
												break;
											}
										case 's':
											{
												av.setAt(j, new StringVar(br.ReadString()));
												break;
											}
										case 'r':
											{
												av.setAt(j, new RealVar(br.ReadDouble()));
												break;
											}
										case 'i':
											{
												av.setAt(j, new IntVar(br.ReadInt32()));
												break;
											}
										default:
											{
												throw new NamespaceSerializationException();
											}
									}
								}
								p.m_var = av;
								break;
							}
						default:
							{
								throw new NamespaceSerializationException();
							}
					}
					m_list.Add(p);
					m_n++;
				}
			}
			catch {
				m_n = 0;
				m_list = new ArrayList();
				throw new NamespaceSerializationException();
			}
			finally {
				br.Close();
			}
		}

		public VariableReport[] GetVariableList() {
			VariableReport[] res = new VariableReport[m_n];
			for (int i = 0; i < m_n; i++) {
				res[i].val = (m_list[i] as Pair).m_var.ToString();
				res[i].name = (m_list[i] as Pair).m_str;
			}
			for (int i = 0; i < m_n - 1; i++) {
				int k = i;
				for (int j = i + 1; j < m_n; j++)
					k = (string.Compare(res[k].name, res[j].name) > 0) ? j : k;
				if (i != k) {
					VariableReport temp = res[k];
					res[k] = res[i];
					res[i] = temp;
				}
			}
			return res;
		}
	}
}