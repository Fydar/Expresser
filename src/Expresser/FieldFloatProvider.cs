using System.Reflection;

namespace ExpressionMathmatics
{
	/// <summary>
	/// <para></para>
	/// </summary>
	public struct FieldFloatProvider : IValueProvider, IObjectContext
	{
		private readonly FieldInfo targetField;

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns>
		/// <para></para>
		/// </returns>
		public MathValue Value => new MathValue((float)targetField.GetValue(Target), false);

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public object Target { get; set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetField"></param>
		public FieldFloatProvider (FieldInfo targetField)
		{
			this.targetField = targetField;
			Target = null;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetField"></param>
		/// <param name="targetObject"></param>
		public FieldFloatProvider (FieldInfo targetField, object targetObject)
		{
			this.targetField = targetField;
			Target = targetObject;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return targetField.Name;
		}
	}
}
