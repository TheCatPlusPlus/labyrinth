using JetBrains.Annotations;

namespace Labyrinth.Data.Ids
{
    public sealed class Id<T> : IId
        where T : IHasId
    {
        public string Value { get; }

        public Id(string value)
        {
            Value = value;
        }

        public bool Equals([NotNull] Id<T> other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            var id = obj as Id<T>;

            if (ReferenceEquals(null, id))
            {
                return false;
            }

            return ReferenceEquals(this, id) || Equals(id);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public static bool operator==([CanBeNull] Id<T> left, [CanBeNull] Id<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator!=([CanBeNull] Id<T> left, [CanBeNull] Id<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
