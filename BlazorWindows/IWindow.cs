﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace KST.Blazor.Windows
{
	public interface IWindow
	{
		public Guid Id { get; }

		public string Title { get; }

		public bool IsDisposed { get; }

		public Task ChangeTitle(string title);
	}

	public interface IWindow<TComponent> : IWindow
		where TComponent : ComponentBase
	{
		public void ChangeParameters(Action<IComponentParameterBag<TComponent>> parameters);
	}
}
