using Newtonsoft.Json;

namespace BookLovers.Base.Infrastructure.Serialization
{
    public static class SerializerSettings
    {
        public static JsonSerializerSettings GetSerializerSettings() => new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new NonPublicContractResolver()
        };
    }
}