namespace Labyrinth.Data.Ids
{
    public class Id<T> : IId
        where T : IHasId
    {
        public string Value { get; }

        public Id(string value)
        {
            Value = value;
        }

        protected bool Equals(Id<T> other)
        {
            return string.Equals(Value, other.Value);
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
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Id<T>)obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator==(Id<T> left, Id<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator!=(Id<T> left, Id<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
