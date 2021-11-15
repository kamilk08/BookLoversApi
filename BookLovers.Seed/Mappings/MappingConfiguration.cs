using System;
using AutoMapper;

namespace BookLovers.Seed.Mappings
{
    internal static class MappingConfiguration
    {
        internal static IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, DateTime?>().ConvertUsing(new DateTimeConverter());
                cfg.AddProfile<ExternalAuthorMapping>();
                cfg.AddProfile<ExternalBookMapping>();
            }).CreateMapper();
        }
    }
}