namespace Expresser.Lexing.Common
{
	public class KeywordTokenClassifier : ITokenClassifier
	{
		private int currentIndex = 0;

		public string Keyword { get; }

		public KeywordTokenClassifier(string keyword)
		{
			Keyword = keyword;
		}

		public void Reset()
		{
			currentIndex = 0;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			string keyword = Keyword;

			if (currentIndex == keyword.Length)
			{
				if (char.IsLetterOrDigit(nextCharacter))
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.GiveUp
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.TokenizeFromLast
					};
				}
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
