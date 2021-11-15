using System.Reflection;
using AutoMapper;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    internal static class MappingConfiguration
    {
        public static IMapper ConfigureMapper()
        {
            return new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()))
                .CreateMapper();
        }
    }
}