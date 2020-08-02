namespace Expresser.Lexing
{
	public enum ClassifierAction : byte
	{
		GiveUp,
		ContinueReading,
		TokenizeFromLast,
		TokenizeImmediately
	}
}
