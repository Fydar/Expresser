using System.Collections.Generic;

namespace ExpressionMathmatics
{
	/// <summary>
	/// <para></para>
	/// </summary>
	public class MathContext : IMathContext
	{
		private readonly IReadOnlyDictionary<string, IValueProvider> Terms;
		private readonly IReadOnlyDictionary<string, IValueProvider> Units;

		public IValueProvider ImplicitReference { get; }

		public MathContext(IReadOnlyDictionary<string, IValueProvider> terms, IReadOnlyDictionary<string, IValueProvider> units, IValueProvider implicitReference)
		{
			Terms = terms;
			Units = units;
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
			return Terms.TryGetValue(key.ToLower(), out provider);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		public bool TryGetUnit(string key, out IValueProvider provider)
		{
			return Units.TryGetValue(key.ToLower(), out provider);
		}
	}
}
