using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

using BearLib;

using Labyrinth.Utils;

namespace Labyrinth.UI.Input
{
    public static class KeyDatabase
    {
        private class Entry
        {
            public int Code { get; }
            public string Name { get; }
            public bool Shifted { get; }

            public Entry(int code, string name, bool shifted)
            {
                Code = code;
                Name = name;
                Shifted = shifted;
            }
        }

        private static readonly Dictionary<string, Entry> ByName;
        private static readonly Dictionary<int, Entry> ByCode;
        private static readonly Dictionary<int, Entry> ByCodeShifted;

        [SuppressMessage("ReSharper", "ArgumentsStyleLiteral")]
        static KeyDatabase()
        {
            ByName = new Dictionary<string, Entry>();
            ByCode = new Dictionary<int, Entry>();
            ByCodeShifted = new Dictionary<int, Entry>();

            // unshifted letters
            Add(Terminal.TK_A, "a");
            Add(Terminal.TK_B, "b");
            Add(Terminal.TK_C, "c");
            Add(Terminal.TK_D, "d");
            Add(Terminal.TK_E, "e");
            Add(Terminal.TK_F, "f");
            Add(Terminal.TK_G, "g");
            Add(Terminal.TK_H, "h");
            Add(Terminal.TK_I, "i");
            Add(Terminal.TK_J, "j");
            Add(Terminal.TK_K, "k");
            Add(Terminal.TK_L, "l");
            Add(Terminal.TK_M, "m");
            Add(Terminal.TK_N, "n");
            Add(Terminal.TK_O, "o");
            Add(Terminal.TK_P, "p");
            Add(Terminal.TK_Q, "q");
            Add(Terminal.TK_R, "r");
            Add(Terminal.TK_S, "s");
            Add(Terminal.TK_T, "t");
            Add(Terminal.TK_U, "u");
            Add(Terminal.TK_V, "v");
            Add(Terminal.TK_W, "w");
            Add(Terminal.TK_X, "x");
            Add(Terminal.TK_Y, "y");
            Add(Terminal.TK_Z, "z");

            // shifted letters
            Add(Terminal.TK_A, "A", shifted: true);
            Add(Terminal.TK_B, "B", shifted: true);
            Add(Terminal.TK_C, "C", shifted: true);
            Add(Terminal.TK_D, "D", shifted: true);
            Add(Terminal.TK_E, "E", shifted: true);
            Add(Terminal.TK_F, "F", shifted: true);
            Add(Terminal.TK_G, "G", shifted: true);
            Add(Terminal.TK_H, "H", shifted: true);
            Add(Terminal.TK_I, "I", shifted: true);
            Add(Terminal.TK_J, "J", shifted: true);
            Add(Terminal.TK_K, "K", shifted: true);
            Add(Terminal.TK_L, "L", shifted: true);
            Add(Terminal.TK_M, "M", shifted: true);
            Add(Terminal.TK_N, "N", shifted: true);
            Add(Terminal.TK_O, "O", shifted: true);
            Add(Terminal.TK_P, "P", shifted: true);
            Add(Terminal.TK_Q, "Q", shifted: true);
            Add(Terminal.TK_R, "R", shifted: true);
            Add(Terminal.TK_S, "S", shifted: true);
            Add(Terminal.TK_T, "T", shifted: true);
            Add(Terminal.TK_U, "U", shifted: true);
            Add(Terminal.TK_V, "V", shifted: true);
            Add(Terminal.TK_W, "W", shifted: true);
            Add(Terminal.TK_X, "X", shifted: true);
            Add(Terminal.TK_Y, "Y", shifted: true);
            Add(Terminal.TK_Z, "Z", shifted: true);

            // unshifted numbers
            Add(Terminal.TK_1, "1");
            Add(Terminal.TK_2, "2");
            Add(Terminal.TK_3, "3");
            Add(Terminal.TK_4, "4");
            Add(Terminal.TK_5, "5");
            Add(Terminal.TK_6, "6");
            Add(Terminal.TK_7, "7");
            Add(Terminal.TK_8, "8");
            Add(Terminal.TK_9, "9");
            Add(Terminal.TK_0, "0");

            // shifted numbers
            Add(Terminal.TK_1, "!", shifted: true);
            Add(Terminal.TK_2, "@", shifted: true);
            Add(Terminal.TK_3, "#", shifted: true);
            Add(Terminal.TK_4, "$", shifted: true);
            Add(Terminal.TK_5, "%", shifted: true);
            Add(Terminal.TK_6, "^", shifted: true);
            Add(Terminal.TK_7, "&", shifted: true);
            Add(Terminal.TK_8, "*", shifted: true);
            Add(Terminal.TK_9, "(", shifted: true);
            Add(Terminal.TK_0, ")", shifted: true);

            // unshifted punctuation
            Add(Terminal.TK_GRAVE, "`");
            Add(Terminal.TK_MINUS, "-");
            Add(Terminal.TK_EQUALS, "=");
            Add(Terminal.TK_LBRACKET, "[");
            Add(Terminal.TK_RBRACKET, "]");
            Add(Terminal.TK_BACKSLASH, "\\");
            Add(Terminal.TK_SEMICOLON, ";");
            Add(Terminal.TK_APOSTROPHE, "'");
            Add(Terminal.TK_COMMA, ",");
            Add(Terminal.TK_PERIOD, ".");
            Add(Terminal.TK_SLASH, "/");

            // shifted punctuation
            Add(Terminal.TK_MINUS, "_", shifted: true);
            Add(Terminal.TK_EQUALS, "+", shifted: true);
            Add(Terminal.TK_LBRACKET, "{", shifted: true);
            Add(Terminal.TK_RBRACKET, "}", shifted: true);
            Add(Terminal.TK_BACKSLASH, "|", shifted: true);
            Add(Terminal.TK_SEMICOLON, ":", shifted: true);
            Add(Terminal.TK_APOSTROPHE, "\"", shifted: true);
            Add(Terminal.TK_GRAVE, "~", shifted: true);
            Add(Terminal.TK_COMMA, "<", shifted: true);
            Add(Terminal.TK_PERIOD, ">", shifted: true);
            Add(Terminal.TK_SLASH, "?", shifted: true);

            // fn
            Add(Terminal.TK_F1, "f1");
            Add(Terminal.TK_F2, "f2");
            Add(Terminal.TK_F3, "f3");
            Add(Terminal.TK_F4, "f4");
            Add(Terminal.TK_F5, "f5");
            Add(Terminal.TK_F6, "f6");
            Add(Terminal.TK_F7, "f7");
            Add(Terminal.TK_F8, "f8");
            Add(Terminal.TK_F9, "f9");
            Add(Terminal.TK_F10, "f10");
            Add(Terminal.TK_F11, "f11");
            Add(Terminal.TK_F12, "f12");

            // named
            Add(Terminal.TK_RETURN, "return");
            Add(Terminal.TK_ENTER, "enter");
            Add(Terminal.TK_ESCAPE, "escape");
            Add(Terminal.TK_BACKSPACE, "backspace");
            Add(Terminal.TK_TAB, "tab");
            Add(Terminal.TK_SPACE, "space");
            Add(Terminal.TK_PAUSE, "pause");
            Add(Terminal.TK_INSERT, "insert");
            Add(Terminal.TK_HOME, "home");
            Add(Terminal.TK_PAGEUP, "page-up");
            Add(Terminal.TK_DELETE, "delete");
            Add(Terminal.TK_END, "end");
            Add(Terminal.TK_PAGEDOWN, "page-down");
            Add(Terminal.TK_RIGHT, "right");
            Add(Terminal.TK_LEFT, "left");
            Add(Terminal.TK_DOWN, "down");
            Add(Terminal.TK_UP, "up");

            // numpad
            Add(Terminal.TK_KP_DIVIDE, "num-/");
            Add(Terminal.TK_KP_MULTIPLY, "num-*");
            Add(Terminal.TK_KP_MINUS, "num--");
            Add(Terminal.TK_KP_PLUS, "num-+");
            Add(Terminal.TK_KP_ENTER, "num-enter");
            Add(Terminal.TK_KP_1, "num-1");
            Add(Terminal.TK_KP_2, "num-2");
            Add(Terminal.TK_KP_3, "num-3");
            Add(Terminal.TK_KP_4, "num-4");
            Add(Terminal.TK_KP_5, "num-5");
            Add(Terminal.TK_KP_6, "num-6");
            Add(Terminal.TK_KP_7, "num-7");
            Add(Terminal.TK_KP_8, "num-8");
            Add(Terminal.TK_KP_9, "num-9");
            Add(Terminal.TK_KP_0, "num-0");
            Add(Terminal.TK_KP_PERIOD, "num-.");

            // mouse
            Add(Terminal.TK_MOUSE_LEFT, "mouse-left");
            Add(Terminal.TK_MOUSE_RIGHT, "mouse-right");
            Add(Terminal.TK_MOUSE_MIDDLE, "mouse-middle");
            Add(Terminal.TK_MOUSE_X1, "mouse-4");
            Add(Terminal.TK_MOUSE_X2, "mouse-5");
        }

        private static void Add(int code, string name, bool shifted = false)
        {
            var entry = new Entry(code, name, shifted);
            ByName[name] = entry;

            if (shifted)
            {
                ByCode[code] = entry;
            }
            else
            {
                ByCodeShifted[code] = entry;
            }
        }

        public static Key Parse(string spec)
        {
            var re = new Regex(@"\s+");
            var parts = re.Split(spec.Trim()).ToList();

            if (!parts.Count.Within(1, 5))
            {
                throw new ArgumentException($"Invalid key spec '{spec}'", nameof(spec));
            }

            var name = parts.Last();
            parts.RemoveAt(parts.Count - 1);

            var shift = false;
            var ctrl = false;
            var alt = false;

            foreach (var mod in parts)
            {
                switch (mod.ToLowerInvariant())
                {
                    case "shift":
                        shift = true;
                        break;
                    case "alt":
                        alt = true;
                        break;
                    case "ctrl":
                        ctrl = true;
                        break;
                    default:
                        throw new ArgumentException($"Invalid modifier '{mod}' in spec '{spec}'", nameof(spec));
                }
            }

            Entry entry;
            if (!ByName.TryGetValue(name, out entry) && !ByName.TryGetValue(name.ToLowerInvariant(), out entry))
            {
                throw new ArgumentException($"Unknown key '{name}' in spec '{spec}'", nameof(spec));
            }

            shift = shift || entry.Shifted;
            return new Key(entry.Code, ctrl, alt, shift);
        }

        public static string Unparse(Key key)
        {
            Dictionary<int, Entry> map;

            if (key.Shift && ByCodeShifted.ContainsKey(key.Code))
            {
                map = ByCodeShifted;
            }
            else if (ByCode.ContainsKey(key.Code))
            {
                map = ByCode;
            }
            else
            {
                throw new ArgumentException($"Invalid key object 0x{key.Code:X04}");
            }

            var spec = new List<string>();
            if (key.Ctrl)
            {
                spec.Add("ctrl");
            }
            if (key.Alt)
            {
                spec.Add("alt");
            }
            if (key.Shift)
            {
                spec.Add("shift");
            }

            var name = map[key.Code].Name;
            spec.Add(name);

            return string.Join(" ", spec).Trim();
        }
    }
}
