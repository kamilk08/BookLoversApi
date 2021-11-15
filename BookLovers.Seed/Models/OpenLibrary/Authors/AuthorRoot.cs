using System.Collections.Generic;
using Newtonsoft.Json;

namespace BookLovers.Seed.Models.OpenLibrary.Authors
{
    public class AuthorRoot
    {
        [JsonProperty("bio")]
        public AuthorBio Bio { get; set; }

        public string Name { get; set; }

        public List<AuthorLink> Links { get; set; }

        [JsonProperty("personal_name")]
        public string PersonalName { get; set; }

        [JsonProperty("death_date")]
        public string DeathDate { get; set; }

        [JsonProperty("alternate_name")]
        public List<string> AlternateNames { get; set; }

        public AuthorCreated Created { get; set; }

        public List<int> Photos { get; set; }

        [JsonProperty("last_modified")]
        public AuthorEdit LastModified { get; set; }

        [JsonProperty("latest_revision")]
        public int LatestRevision { get; set; }

        public string Key { get; set; }

        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        public int Revision { get; set; }

        public AuthorType Type { get; set; }

        [JsonProperty("remote_ids")]
        public AuthorIds RemoteIds { get; set; }
    }
}