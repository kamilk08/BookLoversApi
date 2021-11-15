using System;
using System.Threading.Tasks;

namespace BookLovers.Publication.Domain.Publishers.Services
{
    public interface ISelfPublisherService
    {
        Task<Publisher> GetSelfPublisherAsync();

        Task<Guid> GetSelfPublisherGuidAsync();
    }
}