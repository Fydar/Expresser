using System;
using System.Collections.Generic;
using System.Linq;

namespace Expresser.Language.SimpleMath.Runtime
{
	public class MathContextBuilder : IMathContextBuilder
	{
		private readonly List<KeyValuePair<string, IValueProvider>> terms;
		private readonly List<KeyValuePair<string, IValueProvider>> units;
		private IValueProvider? implicitReference;

		public MathContextBuilder()
		{
			terms = new List<KeyValuePair<string, IValueProvider>>();
			units = new List<KeyValuePair<string, IValueProvider>>();
		}

		public IMathContextBuilder WithTerm(string term, IValueProvider value)
		{
			terms.Add(new KeyValuePair<string, IValueProvider>(term.ToLower(), value));
			return this;
		}

		public IMathContextBuilder WithUnit(string unit, IValueProvider value)
		{
			units.Add(new KeyValuePair<string, IValueProvider>(unit.ToLower(), value));
			return this;
		}

		public IMathContextBuilder ImplicitlyReferences(IValueProvider value)
		{
			if (value != null && implicitReference != null)
			{
				throw new InvalidOperationException("Cannot assign multiple implicit references.");
			}
			implicitReference = value;
			return this;
		}

		public IMathContext Build()
		{
			return new MathContext(
				terms.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
				units.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
				implicitReference
			);
		}
	}
}
