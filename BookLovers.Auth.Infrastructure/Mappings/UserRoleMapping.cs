using AutoMapper;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    internal class UserRoleMapping : Profile
    {
        public UserRoleMapping()
        {
            CreateMap<UserRole, UserRoleReadModel>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(p => p.Role.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(p => p.Role.Name));

            CreateMap<UserRoleReadModel, UserRole>()
                .ConstructUsing(p => new UserRole(new Role(p.Value, p.Name)));
        }
    }
}