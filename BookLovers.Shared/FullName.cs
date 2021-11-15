using BookLovers.Base.Domain.ValueObject;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Shared
{
    public sealed class FullName : ValueObject<FullName>
    {
        public string FirstName { get; }

        public string SecondName { get; }

        public FullName(string fullName)
        {
            var lastSpaceIndex = fullName.LastIndexOf(' ');
            if (lastSpaceIndex == -1 || lastSpaceIndex == 0)
            {
                this.FirstName = string.Empty;
                this.SecondName = string.Empty;
                return;
            }

            FirstName = fullName.Substring(0, lastSpaceIndex);
            SecondName = fullName.Substring(lastSpaceIndex + 1);
        }

        public FullName(string firstName, string secondName)
        {
            FirstName = firstName.IsEmpty() == true ? string.Empty : firstName;
            SecondName = secondName;
        }

        public static FullName Default()
        {
            return new FullName(string.Empty, string.Empty);
        }

        public string GetFullName()
        {
            return FirstName.IsEmpty() ? this.SecondName : string.Concat(FirstName, ' ', SecondName);
        }

        public static string ToFullName(string firstName, string secondName)
        {
            return firstName.IsEmpty() ? secondName : string.Concat(firstName, ' ', secondName);
        }

        protected override bool EqualsCore(FullName obj)
        {
            return this.FirstName == obj.FirstName && this.SecondName == obj.SecondName;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;
            hash = hash * 23 + FirstName.GetHashCode();
            hash = hash * 23 + SecondName.GetHashCode();

            return hash;
        }
    }
}