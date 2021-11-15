using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Ratings.Domain.Publisher
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        Task<Publisher> GetByPublisherGuidAsync(
            Guid publisherGuid);

        Task<Publisher> GetByIdAsync(int publisherId);

        Task<Publisher> GetPublisherWithBookAsync(
            Guid bookGuid);
    }
}