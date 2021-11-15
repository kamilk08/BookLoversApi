using BookLovers.Publication.Domain.Books.Languages;

namespace BookLovers.Publication.Domain.Books
{
    public class BookDetails : BookLovers.Base.Domain.ValueObject.ValueObject<BookDetails>
    {
        public const int MinimalAmountOfPages = 3;

        public int Pages { get; }

        public Language Language { get; }

        private BookDetails()
        {
        }

        public BookDetails(int pages, Language language)
        {
            this.Pages = pages == 0 ? MinimalAmountOfPages : pages;
            this.Language = language;
        }

        public BookDetails(int pages, int language)
        {
            this.Pages = pages == 0 ? MinimalAmountOfPages : pages;
            this.Language = BookLanguages.Get(language);
        }

        protected override bool EqualsCore(BookDetails obj)
        {
            return this.Language == obj.Language && this.Pages == obj.Pages;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Language.GetHashCode();
            hash = (hash * 23) + this.Pages.GetHashCode();

            return hash;
        }
    }
}