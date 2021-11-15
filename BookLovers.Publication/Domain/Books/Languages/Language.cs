using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Domain.Books.Languages
{
    public class Language : Enumeration
    {
        public static readonly Language Unknown = new Language(0, nameof(Unknown));
        public static readonly Language Polish = new Language(1, nameof(Polish));
        public static readonly Language English = new Language(2, nameof(English));

        protected Language()
        {
        }

        protected Language(int value, string name)
            : base(value, name)
        {
        }
    }
}