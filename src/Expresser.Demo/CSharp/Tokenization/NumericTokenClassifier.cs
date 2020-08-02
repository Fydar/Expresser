using Expresser.Syntax;

namespace Expresser.Demo.CSharp.Tokenization
{
	public class NumericTokenClassifier : CharacterCategoryClassifier
	{
		public override bool IsMatched(char character)
		{
			return char.IsDigit(character);
		}
	}
}
