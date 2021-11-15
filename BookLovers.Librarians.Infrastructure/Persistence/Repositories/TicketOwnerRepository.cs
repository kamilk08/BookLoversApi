using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Persistence.Repositories
{
    internal class TicketOwnerRepository : ITicketOwnerRepository, IRepository<TicketOwner>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public TicketOwnerRepository(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<TicketOwner> GetAsync(Guid aggregateGuid)
        {
            var ticketOwner = await this._context.TicketOwners
                .Include(p => p.Tickets).SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return this._mapper.Map<TicketOwnerReadModel, TicketOwner>(ticketOwner);
        }

        public async Task CommitChangesAsync(TicketOwner aggregate)
        {
            var ticketOwner = await this._context.TicketOwners.Include(p => p.Tickets)
                .SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            var mapped = this._mapper.Map(aggregate, ticketOwner);

            this._context.TicketOwners.AddOrUpdate(p => p.Id, mapped);
            await this._context.SaveChangesAsync();
        }

        public TicketOwner GetOwnerByReaderGuid(Guid readerGuid)
        {
            var ticketOwner = this._context.TicketOwners
                .Include(p => p.Tickets)
                .SingleOrDefault(p => p.ReaderGuid == readerGuid);

            return this._mapper.Map<TicketOwnerReadModel, TicketOwner>(ticketOwner);
        }

        public async Task<TicketOwner> GetOwnerByReaderGuidAsync(Guid readerGuid)
        {
            var ticketOwner = await this._context.TicketOwners
                .Include(p => p.Tickets).SingleOrDefaultAsync(p => p.ReaderGuid == readerGuid);

            return this._mapper.Map<TicketOwnerReadModel, TicketOwner>(ticketOwner);
        }

        public async Task<TicketOwner> GetOwnerById(int readerId)
        {
            var ticketOwner = await this._context.TicketOwners
                .Include(p => p.Tickets).SingleOrDefaultAsync(p => p.ReaderId == readerId);

            return this._mapper.Map<TicketOwnerReadModel, TicketOwner>(ticketOwner);
        }
    }
}