namespace BookLovers.Publication.Domain.Books.IsbnValidation
{
    public interface IIsbnValidator
    {
        IsbnType Type { get; }

        bool IsIsbnValid(string isbn);
    }
}