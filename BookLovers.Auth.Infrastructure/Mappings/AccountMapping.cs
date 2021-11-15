using AutoMapper;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    internal class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<AccountReadModel, AccountConfirmation>()
                .ConstructUsing(p => new AccountConfirmation(p.IsAccountConfirmed, p.ConfirmationDate));

            CreateMap<AccountReadModel, AccountDetails>().ConstructUsing(p =>
                new AccountDetails(p.AccountCreateDate, p.HasBeenBlockedPreviously));

            CreateMap<AccountReadModel, AccountSecurity>()
                .ConstructUsing(p => new AccountSecurity(p.Hash, p.Email, p.IsBlocked));

            CreateMap<AccountReadModel, Email>()
                .ConstructUsing(p => new Email(p.Email));

            CreateMap<AccountReadModel, Account>()
                .ForMember(
                    dest => dest.AccountDetails,
                    opt => opt.MapFrom(p => p))
                .ForMember(
                    dest => dest.AccountSecurity,
                    opt => opt.MapFrom(p => p))
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(p => p))
                .ForMember(
                    dest => dest.AccountConfirmation,
                    opt => opt.MapFrom(p => p));

            CreateMap<Account, AccountReadModel>()
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(p => p.Email.Value))
                .ForMember(
                    dest => dest.Hash,
                    opt => opt.MapFrom(p => p.AccountSecurity.Hash))
                .ForMember(
                    dest => dest.Salt,
                    opt => opt.MapFrom(p => p.AccountSecurity.Salt))
                .ForMember(
                    dest => dest.IsBlocked,
                    opt => opt.MapFrom(p => p.AccountSecurity.IsBlocked))
                .ForMember(
                    dest => dest.AccountCreateDate,
                    opt => opt.MapFrom(p => p.AccountDetails.AccountCreateDate))
                .ForMember(
                    dest => dest.IsAccountConfirmed,
                    opt => opt.MapFrom(p => p.AccountConfirmation.IsConfirmed))
                .ForMember(
                    dest => dest.ConfirmationDate,
                    opt => opt.MapFrom(p => p.AccountConfirmation.ConfirmationDate));
        }
    }
}