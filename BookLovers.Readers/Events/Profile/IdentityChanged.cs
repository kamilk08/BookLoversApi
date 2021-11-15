using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class IdentityChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string FirstName { get; private set; }

        public string SecondName { get; private set; }

        public string FullName { get; private set; }

        public int Sex { get; private set; }

        public string SexName { get; private set; }

        public DateTime BirthDate { get; private set; }

        private IdentityChanged()
        {
        }

        [JsonConstructor]
        protected IdentityChanged(
            Guid guid,
            Guid aggregateGuid,
            string firstName,
            string secondName,
            string fullName,
            int sex,
            string sexName,
            DateTime birthDate)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            FirstName = firstName;
            SecondName = secondName;
            FullName = fullName;
            Sex = sex;
            SexName = sexName;
            BirthDate = birthDate;
        }

        private IdentityChanged(
            Guid aggregateGuid,
            string fullName,
            int sex,
            string sexName,
            DateTime birthDate,
            string firstName,
            string secondName)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            FullName = fullName;
            Sex = sex;
            SexName = sexName;
            BirthDate = birthDate;
            FirstName = firstName;
            SecondName = secondName;
        }

        public static IdentityChanged Initialize()
        {
            return new IdentityChanged();
        }

        public IdentityChanged WithAggregate(Guid aggregateGuid)
        {
            return new IdentityChanged(aggregateGuid, FullName, Sex,
                SexName, BirthDate, FirstName, SecondName);
        }

        public IdentityChanged WithFirstName(string firstName)
        {
            return new IdentityChanged(AggregateGuid, FullName, Sex,
                SexName, BirthDate, firstName, SecondName);
        }

        public IdentityChanged WithSecondName(string secondName)
        {
            return new IdentityChanged(AggregateGuid, FullName, Sex,
                SexName, BirthDate, FirstName, secondName);
        }

        public IdentityChanged WithName(string fullName)
        {
            return new IdentityChanged(AggregateGuid, fullName, Sex, SexName,
                BirthDate, FirstName, SecondName);
        }

        public IdentityChanged WithSex(int sex, string sexName)
        {
            return new IdentityChanged(AggregateGuid, FullName, sex,
                sexName, BirthDate, FirstName, SecondName);
        }

        public IdentityChanged WithBirthDate(DateTime birthDate)
        {
            return new IdentityChanged(AggregateGuid, FullName, Sex,
                SexName, birthDate, FirstName, SecondName);
        }
    }
}