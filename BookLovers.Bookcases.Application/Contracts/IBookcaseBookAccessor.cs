using System;
using System.Threading.Tasks;

namespace BookLovers.Bookcases.Application.Contracts
{
    public interface IBookcaseBookAccessor
    {
        Task<Guid> GetBookcaseBookAggregateGuid(Guid bookGuid);
    }
}