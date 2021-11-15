using System;
using System.Linq;
using BookLovers.Base.Domain.ValueObject;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Readers.Domain.Profiles
{
    public class Identity : ValueObject<Identity>
    {
        public FullName FullName { get; }

        public DateTime BirthDate { get; }

        public Sex Sex { get; }

        private Identity()
        {
        }

        public Identity(FullName fullName, Sex sex, DateTime birthDate)
        {
            FullName = fullName;
            Sex = sex;
            BirthDate = birthDate;
        }

        public Identity(FullName fullName, int sexId, DateTime birthDate)
        {
            FullName = fullName;
            BirthDate = birthDate;
            Sex = Sexes.Get(sexId);
        }

        public Identity(string fullName, int sexId, DateTime birthDate)
        {
            var split = fullName?.Split(' ');
            this.FullName = new FullName(split?.First(), split?.Last());
            this.Sex = Sexes.Get(sexId);
            this.BirthDate = birthDate;
        }

        public static Identity Default()
        {
            return new Identity(FullName.Default(), Sex.Hidden.Value,
                default(DateTime));
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Sex.GetHashCode();
            hash = (hash * 23) + this.BirthDate.GetHashCode();
            hash = (hash * 23) + this.FullName.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(Identity obj)
        {
            return BirthDate == obj.BirthDate
                   && FullName == obj.FullName && Sex == obj.Sex;
        }
    }
}