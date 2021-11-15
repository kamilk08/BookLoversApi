using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Seed.Models
{
    public class SourceType : Enumeration
    {
        public static readonly SourceType OpenLibrary = new SourceType(1, nameof(OpenLibrary));
        public static readonly SourceType OwnSource = new SourceType(2, "Own resources");

        protected SourceType(int value, string name)
            : base(value, name)
        {
        }
    }
}