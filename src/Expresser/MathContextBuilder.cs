using System;
using System.Collections.Generic;

namespace ExpressionMathmatics
{
	public class MathContextBuilder : IMathContextBuilder
	{
		private readonly List<KeyValuePair<string, IValueProvider>> Terms;
		private readonly List<KeyValuePair<string, IValueProvider>> Units;
		private IValueProvider ImplicitReference;

		public MathContextBuilder()
		{
			Terms = new List<KeyValuePair<string, IValueProvider>>();
			Units = new List<KeyValuePair<string, IValueProvider>>();
		}

		public IMathContextBuilder WithTerm(string term, IValueProvider value)
		{
			Terms.Add(new KeyValuePair<string, IValueProvider>(term.ToLower(), value));
			return this;
		}

		public IMathContextBuilder WithUnit(string unit, IValueProvider value)
		{
			Units.Add(new KeyValuePair<string, IValueProvider>(unit.ToLower(), value));
			return this;
		}

		public IMathContextBuilder ImplicitlyReferences(IValueProvider value)
		{
			if (value != null && ImplicitReference != null)
			{
				throw new InvalidOperationException("Cannot assign multiple implicit references.");
			}
			ImplicitReference = value;
			return this;
		}

		public IMathContext Build()
		{
			return new MathContext(
				new Dictionary<string, IValueProvider>(Terms),
				new Dictionary<string, IValueProvider>(Units),
				ImplicitReference
			);
		}
	}
}
