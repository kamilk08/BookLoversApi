using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Domain.Authors.Services.Editors
{
    public class AuthorBooksEditor
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorBooksEditor(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task Edit(Author author, AuthorData authorDto)
        {
            foreach (var guid in authorDto.AuthorBooks)
            {
                var book = await this._unitOfWork.GetAsync<Book>(guid);
                if (!book.IsActive())
                    throw new BusinessRuleNotMetException($"Book with GUID {guid} does not exist.");
            }

            var oldSequence = author.Books.AsEnumerable();
            var newSequence = authorDto.AuthorBooks.Select(s => new AuthorBook(s)).AsEnumerable();

            Distinguisher<AuthorBook>.ToRemove(oldSequence, newSequence).ToList()
                .ForEach(author.RemoveBook);

            Distinguisher<AuthorBook>.ToAdd(oldSequence, newSequence).ToList()
                .ForEach(author.AddBook);
        }
    }
}