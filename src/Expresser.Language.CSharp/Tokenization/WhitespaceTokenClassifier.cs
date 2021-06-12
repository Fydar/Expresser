namespace Expresser.Language.CSharp.Tokenization
{
	internal class WhitespaceTokenClassifier : CharacterCategoryTokenClassifier
	{
		public override bool IsMatched(char character)
		{
			return char.IsWhiteSpace(character);
		}
	}
}
