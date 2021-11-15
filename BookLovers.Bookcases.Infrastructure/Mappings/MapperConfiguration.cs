using AutoMapper;

namespace BookLovers.Bookcases.Infrastructure.Mappings
{
    public static class MapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            return new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookcaseMapping>();
                cfg.AddProfile<ShelfMapping>();
                cfg.AddProfile<BookMapping>();
                cfg.AddProfile<BookcaseSettingsMapping>();
                cfg.AddProfile<BookShelfRecordMapping>();
            }).CreateMapper();
        }
    }
}