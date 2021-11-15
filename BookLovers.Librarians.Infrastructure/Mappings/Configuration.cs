using AutoMapper;
using System.Reflection;

namespace BookLovers.Librarians.Infrastructure.Mappings
{
    public static class Configuration
    {
        public static IMapper Configure() => new MapperConfiguration(cfg => cfg
            .AddMaps(Assembly.GetExecutingAssembly())).CreateMapper();
    }
}