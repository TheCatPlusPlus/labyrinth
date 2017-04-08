using BearLib;

using JetBrains.Annotations;

using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI.Input
{
    public abstract class Event
    {
        [CanBeNull]
        public static Event ReadNext()
        {
            var code = Terminal.Read();

            switch (code)
            {
                case Terminal.TK_CLOSE:
                    return new CloseEvent();
                case Terminal.TK_RESIZED:
                    return new ResizeEvent();
                case Terminal.TK_MOUSE_MOVE:
                    var x = Terminal.State(Terminal.TK_MOUSE_X);
                    var y = Terminal.State(Terminal.TK_MOUSE_Y);
                    return new MouseMoveEvent(new Vector2I(x, y));
                case Terminal.TK_MOUSE_SCROLL:
                    var wheel = Terminal.State(Terminal.TK_MOUSE_WHEEL);
                    return new MouseScrollEvent(wheel);
                case Terminal.TK_SHIFT:
                case Terminal.TK_CONTROL:
                case Terminal.TK_ALT:
                    break;
                default:
                    if ((code & Terminal.TK_KEY_RELEASED) != 0)
                    {
                        break;
                    }

                    var shift = Terminal.Check(Terminal.TK_SHIFT);
                    var ctrl = Terminal.Check(Terminal.TK_CONTROL);
                    var alt = Terminal.Check(Terminal.TK_ALT);
                    var key = new Key(code, ctrl, alt, shift);
                    return new KeyEvent(key);
            }

            return null;
        }

        public static bool HasNext()
        {
            var next = Terminal.Peek();
            // things that need HasNext don't want to be interrupted
            // by mouse moves
            return (next != 0) && (next != Terminal.TK_MOUSE_MOVE);
        }
    }

    public sealed class KeyEvent : Event
    {
        public Key Key { get; }

        public KeyEvent(Key key)
        {
            Key = key;
        }

        public override string ToString()
        {
            return $"KeyEvent: {Key}";
        }
    }

    public sealed class MouseScrollEvent : Event
    {
        public int Wheel { get; }

        public MouseScrollEvent(int wheel)
        {
            Wheel = wheel;
        }

        public override string ToString()
        {
            return $"MouseScrollEvent: {Wheel}";
        }
    }

    public sealed class MouseMoveEvent : Event
    {
        public Vector2I Cursor { get; }

        public MouseMoveEvent(Vector2I cursor)
        {
            Cursor = cursor;
        }

        public override string ToString()
        {
            return $"MouseMoveEvent: {Cursor}";
        }
    }

    public sealed class ResizeEvent : Event
    {
        public override string ToString()
        {
            return "ResizeEvent";
        }
    }

    public sealed class CloseEvent : Event
    {
        public override string ToString()
        {
            return "CloseEvent";
        }
    }
}
