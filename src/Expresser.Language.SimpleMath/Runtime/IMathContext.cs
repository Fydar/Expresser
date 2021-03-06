namespace Expresser.Language.SimpleMath.Runtime
{
	public interface IMathContext
	{
		IValueProvider? ImplicitReference { get; }

		bool TryGetTerm(string key, out IValueProvider provider);

		bool TryGetUnit(string key, out IValueProvider provider);

	}
}
