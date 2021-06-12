namespace Expresser.Lexing.Common
{
	public class MultiCharacterOpTokenClassifier : ITokenClassifier
	{
		private int currentIndex = 0;

		public string MatchedString { get; }

		public MultiCharacterOpTokenClassifier(string matchedString)
		{
			MatchedString = matchedString;
		}

		/// <inheritdoc/>
		public void Reset()
		{
			currentIndex = 0;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			string keyword = MatchedString;

			if (currentIndex == keyword.Length)
			{
				return ClassifierAction.TokenizeFromLast();
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
