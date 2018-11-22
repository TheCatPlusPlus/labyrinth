using System;

using BearLib;

using NLog;

namespace Labyrinth.UI
{
	public sealed class ConfirmDialog : BoxDialog
	{
		private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		protected override string Message { get; }
		protected override string Footer { get; } = "[[[color=green]Y[/color]]]es / [[[color=green]N[/color]]]o";

		public event Action Yes;
		public event Action No;

		public ConfirmDialog(GamePrev game, UI ui, string message)
			: base(game, ui)
		{
			Message = message;
		}

		public override DialogResult React(Code code)
		{
			switch (code)
			{
				case Code.Y:
					OnYes();
					return DialogResult.Close;
				case Code.N:
					OnNo();
					return DialogResult.Close;
				default:
					return DialogResult.StayOpen;
			}
		}

		private void OnYes()
		{
			Log.Debug($"{this}: Yes");
			Yes?.Invoke();
		}

		private void OnNo()
		{
			Log.Debug($"{this}: No");
			No?.Invoke();
		}
	}
}
