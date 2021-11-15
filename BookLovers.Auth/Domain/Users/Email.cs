using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Users
{
    public sealed class Email : ValueObject<Email>
    {
        public string Value { get; private set; }

        private Email()
        {
        }

        public Email(string email) => Value = email;

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + Value.GetHashCode();
        }

        protected override bool EqualsCore(Email obj) => Value == obj.Value;
    }
}