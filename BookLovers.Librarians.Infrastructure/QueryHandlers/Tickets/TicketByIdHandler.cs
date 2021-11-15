using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries.Tickets;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.Tickets
{
    internal class TicketByIdHandler : IQueryHandler<TicketByIdQuery, TicketDto>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public TicketByIdHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<TicketDto> HandleAsync(TicketByIdQuery query)
        {
            var ticket = await this._context.Tickets.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == query.TicketId);

            return this._mapper.Map<TicketReadModel, TicketDto>(ticket);
        }
    }
}