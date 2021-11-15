using System;
using System.Threading.Tasks;

namespace BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks
{
    public interface IBookcaseBookAccessor
    {
        Task<Guid> GetBookcaseBookAggregateGuid(Guid bookGuid);
    }
}