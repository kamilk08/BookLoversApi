using System;

namespace BookLovers.Auth.Application.WriteModels
{
    public class AddAudienceWriteModel
    {
        public Guid AudienceGuid { get; set; }

        public long TokenLifeTime { get; set; }

        public string AudienceSecretKey { get; set; }
    }
}