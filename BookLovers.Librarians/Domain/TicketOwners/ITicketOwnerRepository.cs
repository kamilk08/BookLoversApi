using System;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Domain.TicketOwners
{
    public interface ITicketOwnerRepository
    {
        TicketOwner GetOwnerByReaderGuid(Guid readerGuid);

        Task<TicketOwner> GetOwnerByReaderGuidAsync(Guid readerGuid);

        Task<TicketOwner> GetOwnerById(int readerId);
    }
}