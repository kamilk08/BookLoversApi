using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.Profiles
{
    public class FavouriteType : Enumeration
    {
        public static readonly FavouriteType FavouriteBook = new FavouriteType(1, "Favourite book");
        public static readonly FavouriteType FavouriteAuthor = new FavouriteType(2, "Favourite author");

        [JsonConstructor]
        protected FavouriteType(byte value, string name)
            : base(value, name)
        {
        }

        public FavouriteType()
        {
        }
    }
}