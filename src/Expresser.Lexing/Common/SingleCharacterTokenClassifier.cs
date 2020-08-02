namespace Expresser.Lexing.Common
{
	public class SingleCharacterTokenClassifier : ITokenClassifier
	{
		public char MatchedCharacter { get; }

		public SingleCharacterTokenClassifier(char matchedCharacter)
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
					Action = ClassifierAction.TokenizeImmediately
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

		public void Reset()
		{

		}
	}
}
