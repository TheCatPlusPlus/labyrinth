using System.Drawing;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Data.Ids;
using Labyrinth.Maps;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI
{
    public sealed class Viewport
    {
        private static Vector2I Player => State.Game.Player.Position;
        private static Level Level => State.Game.Level;

        private readonly Rect _rect;
        private Vector2I _cursor;

        public Vector2I Cursor
        {
            get => _cursor;
            set => _cursor = _rect.Contains(value) ? value : GridPoint.Invalid;
        }

        public Viewport(Rect rect)
        {
            _rect = rect;
            _cursor = GridPoint.Invalid;
        }

        public Vector2I ScreenToMap(Vector2I screen)
        {
            var mapOrigin = GetMapOrigin();
            var screenOrigin = _rect.Origin;

            var delta = screen - screenOrigin;
            var map = mapOrigin + delta;

            if (!_rect.Contains(screen))
            {
                throw new OutOfBounds(screen, "Requested point outside of the viewport");
            }

            var _ = Level[map];
            return map;
        }

        public Vector2I MapToScreen(Vector2I map)
        {
            var mapOrigin = GetMapOrigin();
            var screenOrigin = _rect.Origin;

            var delta = map - mapOrigin;
            var screen = screenOrigin + delta;

            var _ = Level[map];
            if (!_rect.Contains(screen))
            {
                throw new OutOfBounds(screen, "Translated point outside of the viewport");
            }

            return screen;
        }

        public void Draw()
        {
            var mapOrigin = GetMapOrigin();

            foreach (var p in _rect.RelativePoints)
            {
                var map = mapOrigin + p;
                var screen = _rect.Origin + p;

                if (Level.Rect.Contains(map))
                {
                    Draw(screen, Level[map]);
                }
                else
                {
                    PutGlyph(screen, 0, TileData.OutOfBounds);
                }
            }

            if (_cursor != GridPoint.Invalid)
            {
                using (TerminalExt.Foreground(Color.Red))
                using (TerminalExt.Layer(3))
                {
                    Terminal.Put(_cursor, '\u25AF');
                }
            }
        }

        private static void Draw(Vector2I screen, [NotNull] Tile tile)
        {
            var glyph = GlyphData.For(tile.Id);

            Color? fg, bg;

            if (tile.IsLit)
            {
                fg = glyph.Foreground;
                bg = glyph.Background;
            }
            else
            {
                fg = glyph.UnlitForeground;
                bg = glyph.UnlitBackground;
            }

            var occupied = false;
            if (tile.IsLit)
            {
                occupied = true;

                if (tile.Monster != null)
                {
                    PutGlyph(screen, 2, tile.Monster.Id);
                }
                else if (tile.Items.Count == 1)
                {
                    PutGlyph(screen, 2, tile.Items[0].Id);
                }
                else if (tile.Items.Count > 0)
                {
                    PutGlyph(screen, 2, ItemData.Multiple);
                }
                else
                {
                    occupied = false;
                }
            }

            if (tile.WasSeen && (!occupied || glyph.AlwaysDrawBackground))
            {
                PutGlyph(screen, 0, tile.Id, fg, bg);
            }
        }

        private static void PutGlyph(Vector2I screen, int layer, [NotNull] IId id, Color? fg = null, Color? bg = null)
        {
            var glyph = GlyphData.For(id);
            fg = fg ?? glyph.Foreground;
            bg = bg ?? glyph.Background;

            using (TerminalExt.Colors(fg, bg))
            using (TerminalExt.Layer(layer))
            {
                Terminal.Put(screen, glyph.Glyph);
            }
        }

        private Vector2I GetMapOrigin()
        {
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

            if (Level.Rect.Width > _rect.Width)
            {
                x0 = Go(Player.X, _rect.Width, Level.Rect.Width);
            }

            if (Level.Rect.Height > _rect.Height)
            {
                y0 = Go(Player.Y, _rect.Height, Level.Rect.Height);
            }

            return new Vector2I(x0, y0);
        }
    }
}
