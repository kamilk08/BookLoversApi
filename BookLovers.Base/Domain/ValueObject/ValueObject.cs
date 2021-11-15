namespace BookLovers.Base.Domain.ValueObject
{
    public abstract class ValueObject<T>
        where T : class
    {
        public override bool Equals(object obj)
        {
            if (obj is T)
            {
                var valueObject = obj as T;

                // CHECKING IF REFERENCE OF OBJECT equals to null.
                // If Yes return false and dont procced further
                if (ReferenceEquals(obj, null))
                    return false;

                return EqualsCore(valueObject);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        public static bool operator ==(ValueObject<T> firstObject, ValueObject<T> secondObject)
        {
            if (ReferenceEquals(firstObject, null) && ReferenceEquals(secondObject, null))
                return true;

            if (ReferenceEquals(firstObject, null) || ReferenceEquals(secondObject, null))
                return false;

            return firstObject.Equals(secondObject);
        }

        public static bool operator !=(ValueObject<T> firstObject, ValueObject<T> secondObject)
        {
            return !(firstObject == secondObject);
        }

        protected abstract int GetHashCodeCore();

        protected abstract bool EqualsCore(T obj);
    }
}