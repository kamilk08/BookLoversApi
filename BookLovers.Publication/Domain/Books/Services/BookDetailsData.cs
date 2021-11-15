using BookLovers.Publication.Domain.Books.Languages;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookDetailsData
    {
        public int Pages { get; }

        public Language Language { get; }

        public BookDetailsData(int page, int languageId)
        {
            this.Pages = page;
            this.Language = BookLanguages.Get(languageId);
        }

        public BookDetailsData(int pages, Language language)
        {
            this.Pages = pages;
            this.Language = language;
        }
    }
}