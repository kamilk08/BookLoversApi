using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.Readers
{
    public class AddedResourceType : Enumeration
    {
        public static readonly AddedResourceType Book = new AddedResourceType(1, nameof(Book));
        public static readonly AddedResourceType Author = new AddedResourceType(2, nameof(Author));
        public static readonly AddedResourceType Review = new AddedResourceType(3, nameof(Review));

        [JsonConstructor]
        protected AddedResourceType(byte value, string name)
            : base(value, name)
        {
        }

        public AddedResourceType()
        {
        }
    }
}