namespace Expresser.Lexing.Common
{
	public abstract class CharacterCategoryTokenClassifier : ITokenClassifier
	{
		protected bool IsFirstCharacter { get; private set; }

		public void Reset()
		{
			IsFirstCharacter = true;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			bool isMatched = IsMatched(nextCharacter);

			if (!IsFirstCharacter)
			{
				if (!isMatched)
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.TokenizeFromLast
					};
				}
			}

			IsFirstCharacter = false;

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

		public abstract bool IsMatched(char character);
	}
}
