using AutoMapper;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.ReviewReportRegisters
{
    public class ReportRegisterItemMapping : Profile
    {
        public ReportRegisterItemMapping()
        {
            this.CreateMap<ReportRegisterItem, ReportRegisterItemReadModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ReportReasonId, opt => opt.MapFrom(p => p.ReportReason.Value))
                .ForMember(dest => dest.ReportReasonName, opt => opt.MapFrom(p => p.ReportReason.Name))
                .ForMember(dest => dest.ReportedByGuid, opt => opt.MapFrom(p => p.ReportedBy));

            this.CreateMap<ReportRegisterItemReadModel, ReportRegisterItem>()
                .ConstructUsing(p =>
                new ReportRegisterItem(p.ReportedByGuid, new ReportReason(p.ReportReasonId, p.ReportReasonName)));
        }
    }
}