using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Users
{
    public class UserName : ValueObject<UserName>
    {
        public string Value { get; private set; }

        private UserName()
        {
        }

        public UserName(string value) => Value = value;

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + Value.GetHashCode();
        }

        protected override bool EqualsCore(UserName obj) => Value == obj.Value;
    }
}