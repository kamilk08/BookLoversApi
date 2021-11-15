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
    internal class LibrarianByGuidHandler : IQueryHandler<LibrarianByGuidQuery, LibrarianDto>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public LibrarianByGuidHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<LibrarianDto> HandleAsync(LibrarianByGuidQuery query)
        {
            var librarian = await this._context.Librarians.AsNoTracking()
                .Include(p => p.Tickets).FirstOrDefaultAsync(p => p.Guid == query.LibrarianGuid);

            return this._mapper.Map<LibrarianReadModel, LibrarianDto>(librarian);
        }
    }
}