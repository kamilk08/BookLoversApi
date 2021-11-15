using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Domain.Quotes
{
    public class QuoteType : Enumeration
    {
        public static readonly QuoteType AuthorQuote = new QuoteType(1, "Author quote");
        public static readonly QuoteType BookQuote = new QuoteType(2, "Book quote");

        private static IReadOnlyList<QuoteType> _quoteTypes =
            typeof(QuoteType).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as QuoteType).ToList();

        private QuoteType()
        {
        }

        protected QuoteType(int value, string name)
            : base(value, name)
        {
        }

        public static QuoteType Get(int quoteTypeId) =>
            QuoteType._quoteTypes.SingleOrDefault(p => p.Value == quoteTypeId);
    }
}