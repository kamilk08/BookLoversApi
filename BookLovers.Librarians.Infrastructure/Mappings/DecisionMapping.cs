using AutoMapper;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings
{
    public class DecisionMapping : Profile
    {
        public DecisionMapping()
        {
            this.CreateMap<Decision, DecisionReadModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(p => p.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(p => p.Value));

            this.CreateMap<DecisionReadModel, Decision>()
                .ConstructUsing(rm => new Decision(rm.Value, rm.Name));
        }
    }
}