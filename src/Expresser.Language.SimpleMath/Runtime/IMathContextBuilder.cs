namespace Expresser.Language.SimpleMath.Runtime
{
	public interface IMathContextBuilder
	{
		IMathContext Build();

		IMathContextBuilder ImplicitlyReferences(IValueProvider value);

		IMathContextBuilder WithTerm(string term, IValueProvider value);

		IMathContextBuilder WithUnit(string unit, IValueProvider value);
	}
}
