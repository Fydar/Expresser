namespace Expresser.Lexing.Demo.CSharp.Tokenization
{
	public abstract class CharacterCategoryTokenClassifier : ITokenClassifier
	{
		protected bool IsFirstCharacter { get; private set; }

		/// <inheritdoc/>
		public void Reset()
		{
			IsFirstCharacter = true;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			bool isMatched = IsMatched(nextCharacter);

			if (!IsFirstCharacter)
			{
				if (!isMatched)
				{
					return ClassifierAction.TokenizeFromLast();
				}
			}

			IsFirstCharacter = false;

			return isMatched
				? ClassifierAction.ContinueReading()
				: ClassifierAction.GiveUp();
		}

		public abstract bool IsMatched(char character);
	}
}
