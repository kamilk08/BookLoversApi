using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Shared
{
    public class LifeLength : ValueObject<LifeLength>
    {
        public DateTime BirthDate { get; }
        
        public DateTime DeathDate { get; }

        private LifeLength(){}

        public LifeLength(DateTime birthDate, DateTime deathDate)
        {
            BirthDate = birthDate;
            DeathDate = deathDate;
        }

        public LifeLength(DateTime? birthDate, DateTime? deathDate)
        {
            this.BirthDate = birthDate.GetValueOrDefault();
            this.DeathDate = deathDate.GetValueOrDefault();
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;
            
            hash = hash * this.BirthDate.GetHashCode();
            hash = hash * this.DeathDate.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(LifeLength obj)
        {
            return this.BirthDate == obj.BirthDate && this.DeathDate == obj.DeathDate;
        }
    }
}