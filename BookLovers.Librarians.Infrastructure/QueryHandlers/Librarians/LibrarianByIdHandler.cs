using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Queries.Librarians;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.Librarians
{
    internal class LibrarianByIdHandler : IQueryHandler<LibrarianByIdQuery, LibrarianDto>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public LibrarianByIdHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<LibrarianDto> HandleAsync(LibrarianByIdQuery query)
        {
            var librarian = await this._context
                .Librarians
                .AsNoTracking()
                .Include(c => c.Tickets)
                .FirstOrDefaultAsync(p => p.Id == query.LibrarianId);

            return this._mapper.Map<LibrarianReadModel, LibrarianDto>(librarian);
        }
    }
}