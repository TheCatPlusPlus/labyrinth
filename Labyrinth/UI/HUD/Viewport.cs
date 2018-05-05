using System.Diagnostics;
using System.Drawing;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Database;
using Labyrinth.Geometry;
using Labyrinth.Map;

namespace Labyrinth.UI.HUD
{
	public sealed class Viewport
	{
		private readonly Game _game;
		private readonly Rect _rect;
		private Int2? _cursor;

		public Int2? Cursor
		{
			get => _cursor;
			set => _cursor = value != null && _rect.Contains(value.Value) ? value : null;
		}

		public Viewport(Game game, Rect rect)
		{
			_game = game;
			_rect = rect;
			_cursor = null;
		}

		public Int2? ScreenToMap(Int2 screen)
		{
			Debug.Assert(_game.Player.Level != null, "_game.Player.Level != null");

			var mapOrigin = GetMapOrigin();
			var screenOrigin = _rect.Origin;

			var delta = screen - screenOrigin;
			var map = mapOrigin + delta;

			if (!_rect.Contains(screen))
			{
				return null;
			}

			if (_game.Player.Level.Grid[map] == null)
			{
				return null;
			}

			return map;
		}

		public Int2? MapToScreen(Int2 map)
		{
			Debug.Assert(_game.Player.Level != null, "_game.Player.Level != null");

			var mapOrigin = GetMapOrigin();
			var screenOrigin = _rect.Origin;

			var delta = map - mapOrigin;
			var screen = screenOrigin + delta;

			if (!_rect.Contains(screen))
			{
				return null;
			}

			if (_game.Player.Level.Grid[map] == null)
			{
				return null;
			}

			return screen;
		}

		public void Draw()
		{
			Debug.Assert(_game.Player.Level != null, "_game.Player.Level != null");

			var grid = _game.Player.Level.Grid;
			var mapOrigin = GetMapOrigin();

			foreach (var p in _rect.RelativePoints)
			{
				var map = mapOrigin + p;
				var screen = _rect.Origin + p;
				var tile = grid[map];

				if (tile != null)
				{
					Draw(screen, tile);
				}
				else
				{
					PutGlyph(screen, 0, DB.Glyphs.OutOfBounds);
				}
			}

			if (_cursor != null)
			{
				using (TerminalExt.Foreground(Color.Red))
				using (TerminalExt.Layer(4))
				{
					Terminal.Put(_cursor.Value, '\u25AF');
				}
			}
		}

		private static void Draw(Int2 screen, [NotNull] Tile tile)
		{
			var glyph = DB.Glyphs.Get(tile.Type);
			var flags = tile.EffectiveFlags;
			var isLit = flags.Contains(TileFlag.Lit);
			var wasSeen = flags.Contains(TileFlag.Seen);

			Color? fg, bg;

			if (isLit)
			{
				fg = glyph.Fore;
				bg = glyph.Back;
			}
			else
			{
				fg = glyph.UnlitFore;
				bg = glyph.UnlitBack;
			}

			var occupied = false;
			if (isLit)
			{
				occupied = true;

				if (tile.Creature != null)
				{
					PutGlyph(screen, 2, DB.Glyphs.Get(tile.Creature.ID));
				}
				else if (tile.Items.Count == 1)
				{
					PutGlyph(screen, 2, DB.Glyphs.Get(tile.Items[0].ID));
				}
				else if (tile.Items.Count > 0)
				{
					PutGlyph(screen, 2, DB.Glyphs.MultipleItems);
				}
				else
				{
					occupied = false;
				}
			}

			if (wasSeen && (!occupied || glyph.AlwaysDrawBack))
			{
				PutGlyph(screen, 0, glyph, fg, bg);
			}
		}

		private static void PutGlyph(Int2 screen, int layer, GlyphData glyphData, Color? fg = null, Color? bg = null)
		{
			fg = fg ?? glyphData.Fore;
			bg = bg ?? glyphData.Back;

			using (TerminalExt.Colors(fg, bg))
			using (TerminalExt.Layer(layer))
			{
				Terminal.Put(screen, glyphData.Code);
			}
		}

		private Int2 GetMapOrigin()
		{
			Debug.Assert(_game.Player.Level != null, "_game.Player.Level != null");
			Debug.Assert(_game.Player.Position != null, "_game.Player.Position != null");

			var level = _game.Player.Level;
			var position = _game.Player.Position.Value;
			var rect = level.Grid.Rect;

			int Go(int focusCoord, int viewCoord, int mapCoord)
			{
				if (focusCoord < viewCoord / 2)
				{
					return 0;
				}

				if (focusCoord >= mapCoord - viewCoord / 2)
				{
					return mapCoord - viewCoord;
				}

				return focusCoord - viewCoord / 2;
			}

			var x0 = 0;
			var y0 = 0;

			if (rect.Width > _rect.Width)
			{
				x0 = Go(position.X, _rect.Width, rect.Width);
			}

			if (rect.Height > _rect.Height)
			{
				y0 = Go(position.Y, _rect.Height, rect.Height);
			}

			return new Int2(x0, y0);
		}
	}
}
