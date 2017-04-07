using System.Drawing;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Data.Ids;
using Labyrinth.Maps;
using Labyrinth.Utils;

namespace Labyrinth.UI
{
    public class Viewport
    {
        private static Point Player => State.Game.Player.Position;
        private static Level Level => State.Game.Level;

        private readonly Rectangle _rect;
        private Point _cursor;

        public Point Cursor
        {
            get => _cursor;
            set => _cursor = _rect.Contains(value) ? value : PointExt.Invalid;
        }

        public Viewport(Rectangle rect)
        {
            _rect = rect;
            _cursor = PointExt.Invalid;
        }

        public Point ScreenToMap(Point screen)
        {
            var mapOrigin = GetMapOrigin();
            var screenOrigin = _rect.Location;

            var delta = new Size(screen.X - screenOrigin.X, screen.Y - screenOrigin.Y);
            var map = mapOrigin + delta;

            if (!_rect.Contains(screen))
            {
                throw new OutOfBounds(screen, "Requested point outside of the viewport");
            }

            var _ = Level[map];
            return map;
        }

        public Point MapToScreen(Point map)
        {
            var mapOrigin = GetMapOrigin();
            var screenOrigin = _rect.Location;

            var delta = new Size(map.X - mapOrigin.X, map.Y - mapOrigin.Y);
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

            foreach (Size p in _rect.RelativePoints())
            {
                var map = mapOrigin + p;
                var screen = _rect.Location + p;

                if (Level.Rect.Contains(map))
                {
                    Draw(screen, Level[map]);
                }
                else
                {
                    PutGlyph(screen, 0, TileData.OutOfBounds);
                }
            }

            if (_cursor != PointExt.Invalid)
            {
                using (TerminalExt.Foreground(Color.Red))
                using (TerminalExt.Layer(1))
                {
                    Terminal.Put(_cursor, '\u25AF');
                }
            }
        }

        private void Draw(Point screen, [NotNull] Tile tile)
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
                if (tile.Monster != null)
                {
                    PutGlyph(screen, 2, tile.Monster.Id);
                    occupied = true;
                }
                else if (tile.Items.Count == 1)
                {
                    PutGlyph(screen, 2, tile.Items[0].Id);
                    occupied = true;
                }
                else if (tile.Items.Count > 0)
                {
                    PutGlyph(screen, 2, ItemData.Multiple);
                    occupied = true;
                }
            }

            if (tile.WasSeen && (!occupied || glyph.AlwaysDrawBackground))
            {
                PutGlyph(screen, 0, tile.Id, fg, bg);
            }
        }

        private void PutGlyph(Point screen, int layer, IId id, Color? fg = null, Color? bg = null)
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

        private Point GetMapOrigin()
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

            return new Point(x0, y0);
        }
    }
}
