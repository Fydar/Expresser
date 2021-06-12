using Expresser.Lexing;

namespace Expresser.Language.CSharp.Tokenization
{
	internal class KeywordTokenClassifier : ITokenClassifier
	{
		private int currentIndex = 0;

		public string Keyword { get; }

		public KeywordTokenClassifier(string keyword)
		{
			Keyword = keyword;
		}

		/// <inheritdoc/>
		public void Reset()
		{
			currentIndex = 0;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			string keyword = Keyword;

			if (currentIndex == keyword.Length)
			{
				return char.IsLetterOrDigit(nextCharacter)
					? ClassifierAction.GiveUp()
					: ClassifierAction.TokenizeFromLast();
			}
			else
			{
				if (nextCharacter == keyword[currentIndex])
				{
					currentIndex++;
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.GiveUp();
				}
			}
		}
	}
}
