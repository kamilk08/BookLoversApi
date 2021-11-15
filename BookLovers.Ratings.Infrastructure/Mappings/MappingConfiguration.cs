using System.Reflection;
using AutoMapper;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    internal static class RatingsMappingConfiguration
    {
        public static IMapper ConfigureMapper()
        {
            return new MapperConfiguration(cfg =>
                cfg.AddMaps(Assembly.GetExecutingAssembly())).CreateMapper();
        }
    }
}