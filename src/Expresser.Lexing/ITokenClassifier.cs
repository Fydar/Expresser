namespace Expresser.Lexing
{
	public interface ITokenClassifier
	{
		NextCharacterResult NextCharacter(char nextCharacter);
		void Reset();
	}
}
