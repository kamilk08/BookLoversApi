using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters
{
    public interface IReviewReportRegisterRepository : IRepository<ReviewReportRegister>
    {
        Task<ReviewReportRegister> GetByReviewGuidAsync(Guid reviewGuid);
    }
}