using System.Collections.Generic;

namespace BookLovers.Readers.Infrastructure.Dtos.Readers
{
    public class ProfilePrivacyDto
    {
        public int ProfileId { get; set; }

        public List<ProfilePrivacyOptionDto> PrivacyOptions { get; set; }
    }
}