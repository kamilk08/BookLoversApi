using System;
using AutoMapper;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    public class ReaderMapping : Profile
    {
        public ReaderMapping()
        {
            this.CreateMap<ReaderCreated, ReaderReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(p => p.UserName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.ReaderStatus))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(p => p.Email))
                .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(p => DateTime.UtcNow));

            this.CreateMap<ReaderReadModel, ReaderDto>()
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid));

            this.CreateMap<ReaderReadModel, int>()
                .ConvertUsing(src => src.ReaderId);
        }
    }
}