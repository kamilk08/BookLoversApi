using System;
using System.Threading.Tasks;

namespace BookLovers.Ratings.Domain
{
    public interface IBookInBookcaseChecker
    {
        Task<bool> IsBookInBookcaseAsync(Guid readerGuid, Guid bookGuid);
    }
}