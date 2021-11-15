using BookLovers.Publication.Domain.Books.CoverTypes;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookCoverData
    {
        public CoverType CoverType { get; }

        public string CoverSource { get; }

        public BookCoverData(int coverTypeId, string coverSource)
        {
            this.CoverType = BookCovers.Get(coverTypeId);
            this.CoverSource = coverSource;
        }

        public BookCoverData(CoverType coverType, string coverSource)
        {
            this.CoverType = coverType;
            this.CoverSource = coverSource;
        }
    }
}