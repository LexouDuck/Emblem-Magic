using System;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.IO;
using Nintenlord.Event_Assembler.Core.Code.Templates;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core
{
	public class RawData<T> : IExpression<T>
	{
		private readonly FilePosition pos;
		private readonly byte[] data;

		public EAExpressionType Type {
			get {
				return EAExpressionType.RawData;
			}
		}

		public FilePosition Position {
			get {
				return pos;
			}
		}

		public byte[] Data {
			get {
				return data;
			}
		}

		public RawData (byte[] data, FilePosition position)
		{
			this.pos = position;
			this.data = data;
		}

		public IEnumerable<IExpression<T>> GetChildren() {
			yield break;
		}
	}
}
