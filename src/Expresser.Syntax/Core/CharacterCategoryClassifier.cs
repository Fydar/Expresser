namespace Expresser.Syntax
{
	public abstract class CharacterCategoryClassifier : ITokenClassifier
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
						Action = ClassificationAction.TokenizeFromLast
					};
				}
			}

			IsFirstCharacter = false;

			if (isMatched)
			{
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

		public abstract bool IsMatched(char character);
	}
}
