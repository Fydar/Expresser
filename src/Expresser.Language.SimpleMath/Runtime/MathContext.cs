using System.Collections.Generic;

namespace Expresser.Language.SimpleMath.Runtime
{
	/// <summary>
	/// <para></para>
	/// </summary>
	public class MathContext : IMathContext
	{
		private readonly IReadOnlyDictionary<string, IValueProvider> terms;
		private readonly IReadOnlyDictionary<string, IValueProvider> units;

		public IValueProvider ImplicitReference { get; }

		public MathContext(IReadOnlyDictionary<string, IValueProvider> terms, IReadOnlyDictionary<string, IValueProvider> units, IValueProvider implicitReference)
		{
			this.terms = terms;
			this.units = units;
			ImplicitReference = implicitReference;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		public bool TryGetTerm(string key, out IValueProvider provider)
		{
			return terms.TryGetValue(key.ToLower(), out provider);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		public bool TryGetUnit(string key, out IValueProvider provider)
		{
			return units.TryGetValue(key.ToLower(), out provider);
		}
	}
}
