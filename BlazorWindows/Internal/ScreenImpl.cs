namespace KST.Blazor.Windows.Internal
{
	internal class ScreenImpl : IScreen
	{
		public ScreenImpl(int left, int top, int width, int height, bool isPrimary)
		{
			this.Left = left;
			this.Top = top;
			this.Width = width;
			this.Height = height;
			this.IsPrimary = isPrimary;
		}

		public int Left { get; }
		public int Top { get; }
		public int Width { get; }
		public int Height { get; }
		public bool IsPrimary { get; }
	}
}
