using System;
using AutoMapper;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    internal class AuthorMapping : Profile
    {
        public AuthorMapping()
        {
            this.CreateMap<AuthorCreated, AuthorReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.AuthorStatus))
                .ForMember(dest => dest.AuthorWebsite, opt => opt.MapFrom(p => p.AuthorWebsite))
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorBooks, opt => opt.Ignore())
                .ForMember(dest => dest.AddedBy, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorFollowers, opt => opt.Ignore())
                .ForMember(dest => dest.Quotes, opt => opt.Ignore());

            this.CreateMap<AuthorReadModel, int>().ConstructUsing(p => p.Id);

            this.CreateMap<AuthorReadModel, Guid>().ConstructUsing(p => p.Guid);

            this.CreateMap<AuthorReadModel, AuthorDto>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.AuthorStatus, opt => opt.MapFrom(p => p.Status))
                .ForMember(
                    dest => dest.AuthorFollowers,
                    opt => opt.MapFrom(p => p.AuthorFollowers))
                .ForMember(
                    dest => dest.AuthorSubCategories,
                    opt => opt.MapFrom(p => p.SubCategories))
                .ForMember(
                    dest => dest.AuthorQuotes,
                    opt => opt.MapFrom(p => p.Quotes))
                .ForMember(dest => dest.AddedByReaderId, opt => opt.MapFrom(p => p.AddedBy.ReaderId));

            this.CreateMap<AuthorFollowerReadModel, AuthorFollowerDto>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(p => p.Author.Id))
                .ForMember(dest => dest.FollowedById, opt => opt.MapFrom(p => p.FollowedBy.ReaderId));
        }
    }
}