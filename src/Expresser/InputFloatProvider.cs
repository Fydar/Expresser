namespace Expresser
{
	/// <summary>
	/// <para></para>
	/// </summary>
	public class InputValueProvider : IValueProvider
	{
		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public MathValue Value { get; set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public InputValueProvider (float value)
		{
			Value = new MathValue (value, false);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public InputValueProvider (bool value)
		{
			Value = new MathValue (value);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
}
