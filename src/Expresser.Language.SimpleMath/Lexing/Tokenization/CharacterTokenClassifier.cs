using Expresser.Lexing;

namespace Expresser.Language.SimpleMath.Lexing.Tokenization
{
	public class CharacterLiteralTokenClassifier : ITokenClassifier
	{
		private bool isFirstCharacter = true;
		private bool isEscaped = false;

		public void Reset()
		{
			isFirstCharacter = true;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			if (isFirstCharacter)
			{
				isFirstCharacter = false;

				if (nextCharacter == '\'')
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading,
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
			else
			{
				if (nextCharacter == '\\')
				{
					isEscaped = true;
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading
					};
				}
				else if (nextCharacter == '\'')
				{
					if (isEscaped)
					{
						isEscaped = false;
						return new NextCharacterResult()
						{
							Action = ClassifierAction.ContinueReading
						};
					}
					else
					{
						return new NextCharacterResult()
						{
							Action = ClassifierAction.TokenizeImmediately
						};
					}
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading
					};
				}
			}
		}
	}
}
