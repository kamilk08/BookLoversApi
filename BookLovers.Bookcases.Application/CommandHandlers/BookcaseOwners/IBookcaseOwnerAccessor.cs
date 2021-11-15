using System;
using System.Threading.Tasks;

namespace BookLovers.Bookcases.Application.CommandHandlers.BookcaseOwners
{
    public interface IBookcaseOwnerAccessor
    {
        Task<Guid> GetOwnerAggregateGuidAsync(Guid bookcaseOwnerGuid);
    }
}