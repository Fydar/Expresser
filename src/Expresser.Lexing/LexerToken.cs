using System;

namespace Expresser.Lexing
{
	public struct LexerToken : IEquatable<LexerToken>
	{
		public int LineNumber { get; }
		public int StartIndex { get; }
		public int Length { get; }
		public int ClassifierIndex { get; }

		public LexerToken(int lineNumber, int startIndex, int length, int classifierIndex)
		{
			LineNumber = lineNumber;
			StartIndex = startIndex;
			Length = length;
			ClassifierIndex = classifierIndex;
		}

		public override bool Equals(object obj)
		{
			return obj is LexerToken token && Equals(token);
		}

		public bool Equals(LexerToken other)
		{
			return LineNumber == other.LineNumber &&
				   StartIndex == other.StartIndex &&
				   Length == other.Length &&
				   ClassifierIndex == other.ClassifierIndex;
		}

		public override int GetHashCode()
		{
			int hashCode = 849123704;
			hashCode = hashCode * -1521134295 + LineNumber.GetHashCode();
			hashCode = hashCode * -1521134295 + StartIndex.GetHashCode();
			hashCode = hashCode * -1521134295 + Length.GetHashCode();
			hashCode = hashCode * -1521134295 + ClassifierIndex.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(LexerToken left, LexerToken right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(LexerToken left, LexerToken right)
		{
			return !(left == right);
		}
	}
}
