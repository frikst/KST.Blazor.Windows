namespace KST.Blazor.Windows.Abstractions
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
