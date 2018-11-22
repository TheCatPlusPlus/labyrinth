namespace Labyrinth.Utils
{
	public static class ObjectExt
	{
		public static T DeepClone<T>(this T value)
		{
			return JSON.Clone(value);
		}
	}
}
