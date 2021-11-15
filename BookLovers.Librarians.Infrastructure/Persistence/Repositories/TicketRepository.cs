using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.Services;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Root;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Persistence.Repositories
{
    internal class TicketRepository : ITicketRepository, IRepository<Ticket>
    {
        private readonly LibrariansContext _context;
        private readonly ReadContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public TicketRepository(
            LibrariansContext context,
            ReadContextAccessor contextAccessor,
            IMapper mapper)
        {
            this._context = context;
            this._contextAccessor = contextAccessor;
            this._mapper = mapper;
        }

        public async Task<Ticket> GetAsync(Guid aggregateGuid)
        {
            var ticket = await this._context.Tickets.SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return this._mapper.Map<TicketReadModel, Ticket>(ticket);
        }

        public async Task CommitChangesAsync(Ticket aggregate)
        {
            var ticketReadModel = await this._context.Tickets.SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            ticketReadModel = this._mapper.Map(aggregate, ticketReadModel);

            this._context.Tickets.AddOrUpdate(p => p.Id, ticketReadModel);

            await this._context.SaveChangesAsync();

            this._contextAccessor.AddReadModelId(aggregate.Guid, ticketReadModel.Id);
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            var ticket = await this._context.Tickets.SingleOrDefaultAsync(p => p.Id == ticketId);

            return this._mapper.Map<TicketReadModel, Ticket>(ticket);
        }
    }
}