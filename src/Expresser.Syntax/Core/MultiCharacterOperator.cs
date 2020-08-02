namespace Expresser.Syntax
{
	public class MultiCharacterOperator : ITokenClassifier
	{
		private int CurrentIndex = 0;

		public string MatchedString { get; }

		public MultiCharacterOperator(string matchedString)
		{
			MatchedString = matchedString;
		}

		public void Reset()
		{
			CurrentIndex = 0;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			string keyword = MatchedString;

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
						Action = ClassificationAction.TokenizeImmediately
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
