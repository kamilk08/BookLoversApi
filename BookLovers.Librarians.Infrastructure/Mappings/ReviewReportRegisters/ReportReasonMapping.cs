using AutoMapper;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.ReviewReportRegisters
{
    internal class ReportReasonMapping : Profile
    {
        public ReportReasonMapping() =>
            this.CreateMap<ReportReasonReadModel, ReportReason>()
                .ConstructUsing(p => new ReportReason(p.ReasonId, p.ReasonName));
    }
}