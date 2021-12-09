namespace KST.Blazor.Windows
{
	public interface IScreen
	{
		int Left { get; }
		int Top { get; }

		int Width { get; }
		int Height { get; }

		bool IsPrimary { get; }
	}
}
