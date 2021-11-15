using AutoMapper;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.ReviewReportRegisters
{
    public class ReviewReportMapping : Profile
    {
        public ReviewReportMapping()
        {
            this.CreateMap<ReviewReportRegisterReadModel, SolvedBy>()
                .ConstructUsing(p => new SolvedBy(p.LibrarianGuid.GetValueOrDefault()));

            this.CreateMap<ReviewReportRegister, ReviewReportRegisterReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.LibrarianGuid, opt => opt.MapFrom(p => p.SolvedBy.LibrarianGuid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.Reports, opt => opt.MapFrom(p => p.ReviewReports));

            this.CreateMap<ReviewReportRegisterReadModel, ReviewReportRegister>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(p => p.SolvedBy, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(ReviewReportRegister.Relations.Reports, opt => opt.MapFrom(p => p.Reports));
        }
    }
}