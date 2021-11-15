namespace BookLovers.Publication.Domain.Books.Services
{
    public interface IIsbnUniquenessChecker
    {
        bool IsUnique(string isbn);
    }
}