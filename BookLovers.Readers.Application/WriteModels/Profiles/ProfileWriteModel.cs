using System;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class ProfileWriteModel
    {
        public Guid ProfileGuid { get; set; }

        public AddressWriteModel Address { get; set; }

        public IdentityWriteModel Identity { get; set; }

        public AboutWriteModel About { get; set; }
    }
}