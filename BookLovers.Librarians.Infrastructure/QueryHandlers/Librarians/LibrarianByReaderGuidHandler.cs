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
    internal class LibrarianByReaderGuidHandler :
        IQueryHandler<LibrarianByReaderGuidQuery, LibrarianDto>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public LibrarianByReaderGuidHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<LibrarianDto> HandleAsync(LibrarianByReaderGuidQuery query)
        {
            var librarian = await this._context.Librarians
                .AsNoTracking()
                .Include(c => c.Tickets).AsNoTracking()
                .FirstOrDefaultAsync(p => p.ReaderGuid == query.Guid);

            return this._mapper.Map<LibrarianReadModel, LibrarianDto>(librarian);
        }
    }
}