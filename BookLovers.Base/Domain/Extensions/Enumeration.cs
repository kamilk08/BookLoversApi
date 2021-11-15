using System;

namespace BookLovers.Base.Domain.Extensions
{
    public abstract class Enumeration : IComparable
    {
        public int Value { get; }

        public string Name { get; }

        protected Enumeration()
        {
        }

        protected Enumeration(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public int CompareTo(object obj)
        {
            var enumerationToCompare = obj as Enumeration;
            return this.Value.CompareTo(enumerationToCompare.Value);
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var enumerationToCompare = obj as Enumeration;
            if (enumerationToCompare == null)
                return false;

            var typeMatches = this.GetType().Equals(enumerationToCompare.GetType());
            var valueMatches = this.Value.Equals(enumerationToCompare.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            // IT COULD BE ALSO A NAME IF WE WANT TO COMPARE BY VALUE AND NAME
            return Value.GetHashCode();
        }

        public static bool operator ==(Enumeration firstObject, Enumeration secondObject)
        {
            if (ReferenceEquals(firstObject, null) && ReferenceEquals(secondObject, null))
                return true;

            if (ReferenceEquals(firstObject, null) || ReferenceEquals(secondObject, null))
                return false;

            return firstObject.Equals(secondObject);
        }

        public static bool operator !=(Enumeration firstObject, Enumeration secondObject)
        {
            return !(firstObject == secondObject);
        }
    }
}