using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Publication.Domain.Books.Languages
{
    public static class BookLanguages
    {
        public static readonly IReadOnlyCollection<Language> Languages =
            typeof(Language).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as Language).ToList();

        public static Language Get(int languageId)
        {
            return BookLanguages
                .Languages.SingleOrDefault(p => p.Value == languageId);
        }

        public static bool Has(Language language)
        {
            return BookLanguages
                .Languages.Contains(language);
        }
    }
}