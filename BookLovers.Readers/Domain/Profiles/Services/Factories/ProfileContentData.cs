using System;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Readers.Domain.Profiles.Services.Factories
{
    public class ProfileContentData
    {
        public FullName FullName { get; }

        public DateTime BirthDate { get; }

        public Sex Sex { get; }

        public ProfileContentData(FullName fullName, DateTime birthDate, Sex sex)
        {
            FullName = fullName;
            BirthDate = birthDate;
            Sex = sex;
        }
    }
}