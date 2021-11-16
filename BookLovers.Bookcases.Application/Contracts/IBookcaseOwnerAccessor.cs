using System;
using System.Threading.Tasks;

namespace BookLovers.Bookcases.Application.Contracts
{
    public interface IBookcaseOwnerAccessor
    {
        Task<Guid> GetOwnerAggregateGuidAsync(Guid bookcaseOwnerGuid);
    }
}