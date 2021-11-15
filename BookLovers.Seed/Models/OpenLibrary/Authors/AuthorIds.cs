using Newtonsoft.Json;

namespace BookLovers.Seed.Models.OpenLibrary.Authors
{
    public class AuthorIds
    {
        public string Viaf { get; set; }

        [JsonProperty("wikidata")] public string WikiData { get; set; }

        [JsonProperty("isni")] public string Isni { get; set; }
    }
}