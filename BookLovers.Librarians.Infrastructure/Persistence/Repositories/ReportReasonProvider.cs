using AutoMapper;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Linq;

namespace BookLovers.Librarians.Infrastructure.Persistence.Repositories
{
    internal class ReportReasonProvider : IReportReasonProvider
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public ReportReasonProvider(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public ReportReason GetReportReason(int reasonId)
        {
            var reportReason = this._context.ReportReasons.Single(p => p.ReasonId == reasonId);

            return this._mapper.Map<ReportReasonReadModel, ReportReason>(reportReason);
        }
    }
}