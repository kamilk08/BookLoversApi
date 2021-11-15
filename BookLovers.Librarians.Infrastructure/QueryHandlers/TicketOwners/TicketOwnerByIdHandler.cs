using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.TicketOwners
{
    internal class TicketOwnerByIdHandler : IQueryHandler<TicketOwnerByIdQuery, TicketOwnerDto>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public TicketOwnerByIdHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<TicketOwnerDto> HandleAsync(TicketOwnerByIdQuery query)
        {
            var ticketOwner = await this._context.TicketOwners.AsNoTracking()
                .SingleOrDefaultAsync(p => p.ReaderId == query.ReaderId);

            return this._mapper.Map<TicketOwnerReadModel, TicketOwnerDto>(ticketOwner);
        }
    }
}