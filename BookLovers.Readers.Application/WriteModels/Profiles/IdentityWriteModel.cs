using System;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class IdentityWriteModel
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FullName => FirstName + ' ' + SecondName;

        public DateTime BirthDate { get; set; }

        public int Sex { get; set; }
    }
}