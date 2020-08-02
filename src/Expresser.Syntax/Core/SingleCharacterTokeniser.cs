namespace Expresser.Syntax
{
	public class SingleCharacterTokeniser : ITokenClassifier
	{
		public char MatchedCharacter { get; }

		public SingleCharacterTokeniser(char matchedCharacter)
		{
			MatchedCharacter = matchedCharacter;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			bool isMatched = nextCharacter == MatchedCharacter;

			if (isMatched)
			{
				return new NextCharacterResult()
				{
					Action = ClassificationAction.TokenizeImmediately
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

		public void Reset()
		{

		}
	}
}
