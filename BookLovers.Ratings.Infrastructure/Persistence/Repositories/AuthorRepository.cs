using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Repositories
{
    public class AuthorRepository : IAuthorRepository, IRepository<Author>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public AuthorRepository(RatingsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Author> GetAsync(Guid aggregateGuid)
        {
            var author = await _context.Authors
                .Include(p => p.Books)
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return _mapper.Map<AuthorReadModel, Author>(author);
        }

        public async Task CommitChangesAsync(Author aggregate)
        {
            var author = await _context.Authors
                .Include(p => p.Books)
                .SingleOrDefaultAsync(
                    p => p.Guid == aggregate.Guid);

            if (author == null)
                await AddNewAuthorReadModel(aggregate);
            else
                await UpdateAuthorReadModel(aggregate, author);
        }

        public async Task<Author> GetByAuthorGuidAsync(Guid authorGuid)
        {
            var author = await _context.Authors
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.AuthorGuid == authorGuid);

            return _mapper.Map<AuthorReadModel, Author>(author);
        }

        public async Task<IEnumerable<Author>> GetMultipleAuthorsAsync(
            IEnumerable<Guid> authorGuides)
        {
            var authors = await _context.Authors
                .Include(p => p.Books.Select(s => s.Ratings))
                .Where(p => authorGuides.Contains(p.AuthorGuid))
                .ToListAsync();

            return _mapper.Map<IEnumerable<AuthorReadModel>, IEnumerable<Author>>(authors);
        }

        public async Task<IEnumerable<Author>> GetAuthorsWithBookAsync(
            Guid bookGuid)
        {
            var author = await _context
                .Authors.Include(p => p.Books.Select(s => s.Ratings))
                .Where(p => p.Books.Any(a => a.BookGuid == bookGuid))
                .ToListAsync();

            return _mapper.Map<List<AuthorReadModel>, List<Author>>(author);
        }

        public async Task<Author> GetByIdAsync(int authorId)
        {
            var author = await _context.Authors
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.AuthorId == authorId);

            return _mapper.Map<AuthorReadModel, Author>(author);
        }

        private async Task AddNewAuthorReadModel(Author author)
        {
            _context.Authors.Add(new AuthorReadModel()
            {
                Guid = author.Guid,
                Status = author.Status,
                AuthorGuid = author.Identification.AuthorGuid,
                AuthorId = author.Identification.AuthorId,
                Average = author.Average(),
                RatingsCount = author.RatingsCount(),
                Books = new List<BookReadModel>()
            });

            await _context.SaveChangesAsync();
        }

        private async Task UpdateAuthorReadModel(Author author, AuthorReadModel readModel)
        {
            readModel.Status = author.Status;
            readModel.Average = author.Average();
            readModel.RatingsCount = author.RatingsCount();
            readModel.Books = GetAuthorBooks(author);

            _context.Authors.AddOrUpdate(p => p.AuthorGuid, readModel);

            await _context.SaveChangesAsync();
        }

        private List<BookReadModel> GetAuthorBooks(Author author)
        {
            var bookReadModelList = new List<BookReadModel>();

            foreach (var book in author.Books)
            {
                var authorBook = _context.Books
                    .Include(p => p.Ratings)
                    .Include(p => p.Authors)
                    .Single(p => p.BookGuid == book.Identification.BookGuid);

                bookReadModelList.Add(authorBook);
            }

            return bookReadModelList;
        }
    }
}