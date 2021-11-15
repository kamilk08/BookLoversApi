using System;
using System.Threading.Tasks;

namespace BookLovers.Publication.Domain.BookReaders
{
    public interface IBookReaderAccessor
    {
        Task<Guid> GetAggregateGuidAsync(Guid bookReaderGuid);
    }
}