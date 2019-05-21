using System;
using System.Collections.Generic;

namespace Expresser
{
	public static class IMathContextExtensions
	{
		public static IValueProvider[] ResolveTerms (this IMathContext context, IReadOnlyList<string> terms)
		{
			int termsCount = terms.Count;
			var providers = new IValueProvider[termsCount];

			for (int i = 0; i < termsCount; i++)
			{
				string term = terms[i];
				IValueProvider provider;
				if (!context.TryGetTerm (term, out provider))
				{
					throw new InvalidOperationException (string.Format ("Unable to find value for term \"{0}\"", term));
				}
				providers[i] = provider;
			}
			return providers;
		}
	}
}
