using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Publication.Domain.Books.CoverTypes
{
    public class BookCovers
    {
        public static readonly IReadOnlyList<CoverType> CoverTypes = typeof(CoverType)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(s => s.GetValue(s) as CoverType).ToList();

        public static CoverType Get(int coverId)
        {
            return BookCovers.CoverTypes
                .SingleOrDefault(p => p.Value == coverId);
        }

        public static bool Has(CoverType coverType)
        {
            return BookCovers.CoverTypes
                .Contains(coverType);
        }
    }
}