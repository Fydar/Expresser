namespace Expresser.Lexing.Demo.CSharp.Tokenization
{
	public class WhitespaceTokenClassifier : CharacterCategoryTokenClassifier
	{
		public override bool IsMatched(char character)
		{
			return char.IsWhiteSpace(character);
		}
	}
}
