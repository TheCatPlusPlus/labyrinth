using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BearLib;

namespace Labyrinth.UI
{
	public static class Tileset
	{
		private const string FontFamily = "SourceSansPro";
		private const int FontSize = 16;

		private const int CellWidth = 8;
		private const int CellHeight = 16;

		private const int TileWidth = 32;
		private const int TileHeight = 32;

		private const int TileHSpacing = TileWidth / CellWidth;
		private const int TileVSpacing = TileHeight / CellHeight;

		private static readonly Dictionary<string, int> Tiles = new Dictionary<string, int>();

		public static void Load()
		{
			var assets = new DirectoryInfo(Data.AssetsPath);

			LoadMainFont("SourceSansPro", ("", "Regular"), ("italic", "Italic"), ("bold", "Bold"), ("bold-italic", "BoldItalic"));

			foreach (var file in assets.EnumerateFiles("*.atlas", SearchOption.AllDirectories))
			{
				var name = Path.GetFileNameWithoutExtension(file.Name);
				var png = Path.ChangeExtension(file.FullName, "png");
				var codepage = Path.ChangeExtension(file.FullName, "codepage");
				var entries = File.ReadAllLines(file.FullName, Encoding.UTF8);
				var first = Convert.ToInt32(entries[0].Trim(), 16);

				LoadTiles(name, png, codepage, first, entries.AsSpan(1, entries.Length - 1));
			}
		}

		private static void LoadMainFont(string family, params (string Name, string Variant)[] variants)
		{
			var fontPath = Path.Combine(Data.AssetsPath, "Fonts", family);

			foreach (var variant in variants)
			{
				var variantPath = Path.Combine(fontPath, $"{family}-{variant.Variant}.ttf");
				Terminal.Set($"{variant.Name} font: {variantPath}, size={FontSize};");
			}
		}

		private static void LoadTiles(string name, string png, string codepage, int first, Span<string> items)
		{
			Terminal.Set($"0x{first:X8}: {png}, codepage={codepage}, align=top-left, spacing={TileHSpacing}x{TileVSpacing};");

			for (var idx = 0; idx < items.Length; ++idx)
			{
				var itemName = items[idx].Trim().Split(' ')[0];
				var codepoint = first + idx;
				var item = $"{name}/{itemName}";
				Tiles[item] = codepoint;
			}
		}

		public static int Get(string name)
		{
			return Tiles[name];
		}
	}
}
