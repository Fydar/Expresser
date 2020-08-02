using Expresser.Syntax;

namespace Expresser.Demo.Json.Tokenization
{
	public class WhitespaceTokenClassifier : CharacterCategoryClassifier
	{
		public override bool IsMatched(char character)
		{
			return char.IsWhiteSpace(character);
		}
	}
}
