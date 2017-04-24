using JetBrains.Annotations;

namespace Labyrinth.UI.Input
{
    public sealed class Key
    {
        public int Code { get; }
        public bool Ctrl { get; }
        public bool Shift { get; }
        public bool Alt { get; }

        [NotNull]
        public Key WithoutMods => new Key(Code, false, false, false);

        public Key(int code, bool ctrl, bool alt, bool shift)
        {
            Code = code;
            Ctrl = ctrl;
            Shift = shift;
            Alt = alt;
        }

        public override string ToString()
        {
            return KeyDatabase.Unparse(this);
        }

        public bool Equals([NotNull] Key other)
        {
            return Code == other.Code && Ctrl == other.Ctrl && Shift == other.Shift && Alt == other.Alt;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            var key = obj as Key;

            if (ReferenceEquals(null, key))
            {
                return false;
            }

            return ReferenceEquals(this, obj) || Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Code;
                hashCode = (hashCode * 397) ^ Ctrl.GetHashCode();
                hashCode = (hashCode * 397) ^ Shift.GetHashCode();
                hashCode = (hashCode * 397) ^ Alt.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator==([CanBeNull] Key left, [CanBeNull] Key right)
        {
            return Equals(left, right);
        }

        public static bool operator!=([CanBeNull] Key left, [CanBeNull] Key right)
        {
            return !Equals(left, right);
        }
    }
}
