namespace Expresser.Input
{
	public interface IMathContext
	{
		IValueProvider ImplicitReference { get; }

		bool TryGetTerm (string key, out IValueProvider provider);

		bool TryGetUnit (string key, out IValueProvider provider);

	}
}
