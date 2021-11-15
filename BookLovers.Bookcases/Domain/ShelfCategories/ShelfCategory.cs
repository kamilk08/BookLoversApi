using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Bookcases.Domain.ShelfCategories
{
    public class ShelfCategory : Enumeration
    {
        public static readonly ShelfCategory Read = new ShelfCategory(1, nameof(Read));
        public static readonly ShelfCategory NowReading = new ShelfCategory(2, "Now reading");
        public static readonly ShelfCategory WantToRead = new ShelfCategory(3, "Want to read");
        public static readonly ShelfCategory Custom = new ShelfCategory(4, nameof(Custom));

        protected ShelfCategory()
        {
        }

        [JsonConstructor]
        protected ShelfCategory(int value, string name)
            : base(value, name)
        {
        }
    }
}