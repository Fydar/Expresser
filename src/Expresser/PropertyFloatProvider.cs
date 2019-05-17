using System.Reflection;

namespace ExpressionMathmatics
{
	/// <summary>
	/// <para></para>
	/// </summary>
	public struct PropertyFloatProvider : IValueProvider, IObjectContext
	{
		private readonly PropertyInfo targetProperty;

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public MathValue Value => new MathValue((float)targetProperty.GetValue(Target), false);

		/// <summary>
		/// <para></para>
		/// </summary>
		public object Target { get; set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetProperty"></param>
		public PropertyFloatProvider (PropertyInfo targetProperty)
		{
			this.targetProperty = targetProperty;
			Target = null;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetProperty"></param>
		/// <param name="targetObject"></param>
		public PropertyFloatProvider (PropertyInfo targetProperty, object targetObject)
		{
			this.targetProperty = targetProperty;
			Target = targetObject;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return targetProperty.Name;
		}
	}
}
