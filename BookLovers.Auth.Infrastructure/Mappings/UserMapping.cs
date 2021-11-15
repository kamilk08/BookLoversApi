using System.Linq;
using AutoMapper;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Infrastructure.Dtos.Users;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    internal class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(p => p.Roles))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(p => p.UserName.Value))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(p => p.Account));

            CreateMap<UserReadModel, UserName>()
                .ConstructUsing(p => new UserName(p.UserName));

            CreateMap<UserReadModel, AccountConfirmation>()
                .ConstructUsing(p => new AccountConfirmation(
                    p.Account.IsAccountConfirmed,
                    p.Account.ConfirmationDate));

            CreateMap<UserReadModel, Email>()
                .ConstructUsing(p => new Email(p.Account.Email));

            CreateMap<UserReadModel, AccountDetails>()
                .ConstructUsing(
                    p => new AccountDetails(
                        p.Account.AccountCreateDate,
                        p.Account.HasBeenBlockedPreviously));

            CreateMap<UserReadModel, AccountSecurity>()
                .ConstructUsing(p => new AccountSecurity(
                    p.Account.Hash,
                    p.Account.Salt,
                    p.Account.IsBlocked));

            CreateMap<UserReadModel, Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.AccountId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.AccountDetails, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.AccountSecurity, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.AccountConfirmation, opt => opt.MapFrom(p => p));

            CreateMap<UserReadModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(p => p))
                .ForMember(User.Relations.Roles, opt => opt.MapFrom(p => p.Roles))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(p => p));

            CreateMap<UserReadModel, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(p => p.Account.Email))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(p => p.Roles.Select(s => s.Name)));
        }
    }
}