using System.Threading.Tasks;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public interface IBookEditor
    {
        Task EditBook(Book book, BookData bookData);
    }
}