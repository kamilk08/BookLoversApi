using BookLovers.Base.Domain.ValueObject;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Domain.Authors
{
    public class AuthorBasics : ValueObject<AuthorBasics>
    {
        public FullName FullName { get; }

        public Sex Sex { get; }

        private AuthorBasics()
        {
        }

        public AuthorBasics(FullName fullName, Sex sex)
        {
            this.FullName = fullName;
            this.Sex = sex;
        }

        public AuthorBasics(byte sexId)
        {
            this.FullName = this.FullName;
            this.Sex = Sexes.Get(sexId);
        }

        protected override bool EqualsCore(AuthorBasics obj) =>
            this.FullName == obj.FullName && this.Sex.Value == obj.Sex.Value;

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.FullName.GetHashCode();
            hash = (hash * 23) + this.Sex.GetHashCode();

            return hash;
        }
    }
}