using AutoMapper;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    public static class AuthMapperConfiguration
    {
        public static IMapper CreateMapper() => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMapping>();
            cfg.AddProfile<AccountMapping>();
            cfg.AddProfile<UserRoleMapping>();
            cfg.AddProfile<TokenMapping>();
            cfg.AddProfile<AudienceMapping>();
            cfg.AddProfile<RegisterSummaryMapping>();
            cfg.AddProfile<PasswordResetTokenMapping>();
        }).CreateMapper();
    }
}