namespace Expresser.Lexing.Demo.Json.Tokenization
{
	public class SingleCharacterTokenClassifier : ITokenClassifier
	{
		public char MatchedCharacter { get; }

		public SingleCharacterTokenClassifier(char matchedCharacter)
		{
			MatchedCharacter = matchedCharacter;
		}

		/// <inheritdoc/>
		public void Reset()
		{
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			bool isMatched = nextCharacter == MatchedCharacter;

			return isMatched
				? ClassifierAction.TokenizeImmediately()
				: ClassifierAction.GiveUp();
		}
	}
}
