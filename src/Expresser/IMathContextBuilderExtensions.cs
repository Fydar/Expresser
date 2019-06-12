namespace Expresser
{
	public static class IMathContextBuilderExtensions
	{
		public static IMathContextBuilder WithTerm (this IMathContextBuilder builder, string term, MathValue mathValue)
		{
			return builder.WithTerm (term, new StaticValueProvider (mathValue));
		}
	}
}
