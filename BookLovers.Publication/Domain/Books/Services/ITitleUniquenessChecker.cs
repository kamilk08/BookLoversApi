namespace BookLovers.Publication.Domain.Books.Services
{
    public interface ITitleUniquenessChecker
    {
        bool IsUnique(string title);
    }
}