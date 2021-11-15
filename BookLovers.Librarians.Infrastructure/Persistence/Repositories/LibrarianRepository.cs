using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using BookLovers.Librarians.Infrastructure.Root;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Persistence.Repositories
{
    internal class LibrarianRepository : ILibrarianRepository, IRepository<Librarian>
    {
        private readonly LibrariansContext _context;
        private readonly ReadContextAccessor _readContextAccessor;
        private readonly IMapper _mapper;

        public LibrarianRepository(
            LibrariansContext context,
            ReadContextAccessor readContextAccessor,
            IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._readContextAccessor = readContextAccessor;
        }

        public async Task<Librarian> GetAsync(Guid aggregateGuid)
        {
            var librarian = await this._context.Librarians.Include(p => p.Tickets)
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return this._mapper.Map<LibrarianReadModel, Librarian>(librarian);
        }

        public async Task CommitChangesAsync(Librarian aggregate)
        {
            var librarian = await this._context.Librarians.Include(p => p.Tickets)
                .SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            librarian = this._mapper.Map(aggregate, librarian);

            this._context.Librarians.AddOrUpdate(p => p.Id, librarian);

            await this._context.SaveChangesAsync();

            this._readContextAccessor.AddReadModelId(aggregate.Guid, librarian.Id);
        }

        public async Task<Librarian> GetLibrarianByReaderGuid(Guid readerGuid)
        {
            var librarian = await this._context.Librarians.Include(p => p.Tickets)
                .SingleOrDefaultAsync(p => p.ReaderGuid == readerGuid);

            return this._mapper.Map<LibrarianReadModel, Librarian>(librarian);
        }
    }
}