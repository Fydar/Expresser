using Expresser.Syntax;

namespace Expresser.Demo.Json.Tokenization
{
	public class NumericTokenClassifier : CharacterCategoryClassifier
	{
		public override bool IsMatched(char character)
		{
			return char.IsDigit(character)
				|| character == '.';
		}
	}
}
