using System.Diagnostics.CodeAnalysis;
using System.Drawing;

using Labyrinth.Data.Ids;
using Labyrinth.Utils;

namespace Labyrinth.Data
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class GlyphData : IHasId
    {
        private static readonly Registry<GlyphData> Registry;

        private static readonly Color DefaultUnlitForeground = Color.FromArgb(0x20, 0x20, 0x20);
        private static readonly Color DefaultUnlitBackground = DefaultUnlitForeground;

        private Color? _unlitFore;

        public IId Id { get; }
        public char Glyph { get; }
        public Color Foreground { get; private set; }
        public Color? Background { get; private set; }
        public bool AlwaysDrawBackground { get; private set; }

        public Color UnlitForeground
        {
            get => _unlitFore ?? DefaultUnlitForeground;
            private set => _unlitFore = value;
        }

        public Color UnlitBackground => Background.HasValue && (Background != Color.Black)
            ? DefaultUnlitBackground
            : Color.Black;

        public GlyphData(IId id, char glyph)
        {
            Id = id;
            Glyph = glyph;
        }

        static GlyphData()
        {
            Registry = new Registry<GlyphData>
            {
                // tile types
                new GlyphData(TileData.Ground, '.')
                {
                    Foreground = Color.FromArgb(0x55, 0x55, 0x55)
                },
                new GlyphData(TileData.Wall, ' ')
                {
                    Background = Color.FromArgb(0x50, 0x50, 0x50)
                },
                new GlyphData(TileData.WallDeep, ' ')
                {
                    Background = Color.Black
                },
                new GlyphData(TileData.DoorClosed, '+')
                {
                    Foreground = Color.FromArgb(0xC9, 0x76, 0x00)
                },
                new GlyphData(TileData.DoorOpen, '/')
                {
                    Foreground = Color.FromArgb(0xC9, 0x76, 0x00)
                },
                new GlyphData(TileData.OutOfBounds, ' ')
                {
                    Background = Color.Black
                },
                new GlyphData(TileData.WaterShallow, '~')
                {
                    Foreground = Color.SteelBlue.Lighten(0.25f),
                    AlwaysDrawBackground = true,
                },
                new GlyphData(TileData.WaterDeep, '~')
                {
                    Foreground = Color.SteelBlue.Darken(0.25f),
                    AlwaysDrawBackground = true,
                },
                new GlyphData(TileData.Lava, '~')
                {
                    Foreground = Color.DarkRed,
                    AlwaysDrawBackground = true,
                },

                // monsters
                new GlyphData(MonsterData.Player, '@')
                {
                    Foreground = Color.White
                },
                new GlyphData(MonsterData.Rodent, 'r')
                {
                    Foreground = Color.LightGray
                },
                new GlyphData(MonsterData.Bat, 'b')
                {
                    Foreground = Color.Brown
                },

                // items
                new GlyphData(ItemData.Multiple, '%')
                {
                    Foreground = Color.White
                },
                new GlyphData(ItemData.Rock, '*')
                {
                    Foreground = Color.SlateGray
                },
                new GlyphData(ItemData.Sword, '|')
                {
                    Foreground = Color.LightSteelBlue
                }
            };
        }

        public static GlyphData For(IId id)
        {
            return Registry.Get(id);
        }
    }
}
