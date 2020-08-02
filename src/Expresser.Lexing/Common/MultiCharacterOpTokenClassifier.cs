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

		public void Reset()
		{
			currentIndex = 0;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			string keyword = MatchedString;

			if (currentIndex == keyword.Length)
			{
				return new NextCharacterResult()
				{
					Action = ClassifierAction.TokenizeFromLast
				};
			}
			else
			{
				if (nextCharacter == keyword[currentIndex])
				{
					currentIndex++;
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.GiveUp
					};
				}
			}
		}
	}
}
