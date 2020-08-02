namespace Expresser.Syntax
{
	public class KeywordTokenClassifier : ITokenClassifier
	{
		private int CurrentIndex = 0;

		public string Keyword { get; }

		public KeywordTokenClassifier(string keyword)
		{
			Keyword = keyword;
		}

		public void Reset()
		{
			CurrentIndex = 0;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			string keyword = Keyword;

			if (CurrentIndex == keyword.Length)
			{
				if (char.IsLetterOrDigit(nextCharacter))
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.GiveUp
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.TokenizeFromLast
					};
				}
			}
			else
			{
				if (nextCharacter == keyword[CurrentIndex])
				{
					CurrentIndex++;
					return new NextCharacterResult()
					{
						Action = ClassificationAction.ContinueReading
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.GiveUp
					};
				}
			}
		}
	}
}
