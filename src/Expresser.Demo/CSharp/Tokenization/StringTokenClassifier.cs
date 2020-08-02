using Expresser.Syntax;

namespace Expresser.Demo.CSharp.Tokenization
{
	public class StringTokenClassifier : ITokenClassifier
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

				if (nextCharacter == '"')
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.ContinueReading,
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
			else
			{
				if (nextCharacter == '\\')
				{
					isEscaped = true;
					return new NextCharacterResult()
					{
						Action = ClassificationAction.ContinueReading
					};
				}
				else if (nextCharacter == '"')
				{
					if (isEscaped)
					{
						isEscaped = false;
						return new NextCharacterResult()
						{
							Action = ClassificationAction.ContinueReading
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
					return new NextCharacterResult()
					{
						Action = ClassificationAction.ContinueReading
					};
				}
			}
		}
	}
}
