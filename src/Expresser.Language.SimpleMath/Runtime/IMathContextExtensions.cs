using System;
using System.Collections.Generic;

namespace Expresser.Language.SimpleMath.Runtime
{
	public static class IMathContextExtensions
	{
		public static IValueProvider[] ResolveTerms(this IMathContext context, IReadOnlyList<string> terms)
		{
			if (context == null)
			{
				if (terms != null && terms.Count != 0)
				{
					throw new InvalidOperationException("Could not resolve terms with no math context provided");
				}

				return null;
			}

			int termsCount = terms.Count;
			var providers = new IValueProvider[termsCount];

			for (int i = 0; i < termsCount; i++)
			{
				string term = terms[i];
				if (!context.TryGetTerm(term, out var provider))
				{
					throw new InvalidOperationException(string.Format("Unable to find value for term \"{0}\"", term));
				}
				providers[i] = provider;
			}
			return providers;
		}
	}
}
