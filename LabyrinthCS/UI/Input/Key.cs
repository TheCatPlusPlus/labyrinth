namespace Labyrinth.UI.Input
{
    public class Key
    {
        public int Code { get; }
        public bool Ctrl { get; }
        public bool Shift { get; }
        public bool Alt { get; }

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

        protected bool Equals(Key other)
        {
            return Code == other.Code && Ctrl == other.Ctrl && Shift == other.Shift && Alt == other.Alt;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Key)obj);
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

        public static bool operator==(Key left, Key right)
        {
            return Equals(left, right);
        }

        public static bool operator!=(Key left, Key right)
        {
            return !Equals(left, right);
        }
    }
}
