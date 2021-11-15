using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Repositories
{
    public class BookRepository : IBookRepository, IRepository<Book>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public BookRepository(RatingsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Book> GetAsync(Guid aggregateGuid)
        {
            var book = await _context.Books
                .Include(p => p.Authors)
                .Include(p => p.Ratings)
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return _mapper.Map<BookReadModel, Book>(book);
        }

        public async Task CommitChangesAsync(Book aggregate)
        {
            var book = await _context.Books
                .Include(p => p.Authors)
                .Include(p => p.Ratings)
                .SingleOrDefaultAsync(p => p.BookGuid == aggregate.Identification.BookGuid);

            if (book == null)
                await AddNewBookReadModelAsync(aggregate);
            else
                await UpdateBookReadModel(aggregate, book);
        }

        public async Task<Book> GetByBookGuidAsync(Guid bookGuid)
        {
            var book = await _context.Books.Include(p => p.Authors)
                .Include(p => p.Ratings).SingleOrDefaultAsync(p => p.BookGuid == bookGuid);

            return _mapper.Map<BookReadModel, Book>(book);
        }

        public async Task<Book> GetByIdAsync(int bookId)
        {
            var book = await _context.Books
                .Include(p => p.Authors)
                .Include(p => p.Ratings)
                .SingleOrDefaultAsync(p => p.BookId == bookId);

            return _mapper.Map<BookReadModel, Book>(book);
        }

        private async Task AddNewBookReadModelAsync(Book book)
        {
            _context.Books.Add(new BookReadModel()
            {
                Guid = book.Guid,
                Authors = GetBookAuthors(book),
                BookGuid = book.Identification.BookGuid,
                BookId = book.Identification.BookId,
                Status = book.Status,
                Average = book.Average(),
                RatingsCount = book.RatingsCount(),
                Ratings = new List<RatingReadModel>()
            });

            await _context.SaveChangesAsync();
        }

        private async Task UpdateBookReadModel(Book book, BookReadModel readModel)
        {
            readModel.Status = book.Status;
            readModel.Authors = GetBookAuthors(book);
            readModel.Average = book.Average();
            readModel.RatingsCount = book.RatingsCount();
            readModel.Ratings = _mapper.Map<IEnumerable<Rating>, List<RatingReadModel>>(book.Ratings);

            await _context.SaveChangesAsync();
        }

        private List<AuthorReadModel> GetBookAuthors(Book book)
        {
            var lst = new List<AuthorReadModel>();

            foreach (var author in book.Authors)
            {
                var authorBook = _context.Authors
                    .Include(p => p.Books)
                    .Single(p => p.AuthorGuid == author.Identification.AuthorGuid);

                lst.Add(authorBook);
            }

            return lst;
        }
    }
}