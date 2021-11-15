using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Domain.Publishers.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Root.Domain
{
    internal class SelfPublisherService : ISelfPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PublicationsContext _context;

        public SelfPublisherService(IUnitOfWork unitOfWork, PublicationsContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<Publisher> GetSelfPublisherAsync()
        {
            var selfPublisher = _context.Publishers.AsNoTracking()
                .Single(
                    p => p.Publisher == SelfPublisher.Key);

            return await _unitOfWork.GetAsync<Publisher>(selfPublisher.Guid);
        }

        public Task<Guid> GetSelfPublisherGuidAsync()
        {
            return _context.Publishers.AsNoTracking()
                .Where(p => p.Publisher == SelfPublisher.Key)
                .Select(s => s.Guid)
                .SingleAsync();
        }
    }
}