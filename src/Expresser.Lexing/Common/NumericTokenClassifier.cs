namespace Expresser.Lexing.Common
{
	public class NumericTokenClassifier : ITokenClassifier
	{
		private bool isFirstCharacter;

		public void Reset()
		{
			isFirstCharacter = true;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			bool isMatched = char.IsDigit(nextCharacter)
				|| nextCharacter == '.';

			if (!isFirstCharacter)
			{
				if (!isMatched)
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.TokenizeFromLast
					};
				}
			}
			else if (nextCharacter == '-')
			{
				return new NextCharacterResult()
				{
					Action = ClassifierAction.ContinueReading
				};
			}

			isFirstCharacter = false;

			if (isMatched)
			{
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
