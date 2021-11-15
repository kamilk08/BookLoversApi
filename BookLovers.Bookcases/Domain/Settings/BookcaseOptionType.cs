using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Bookcases.Domain.Settings
{
    public class BookcaseOptionType : Enumeration
    {
        public static readonly BookcaseOptionType Privacy = new BookcaseOptionType(1, nameof(Privacy));
        public static readonly BookcaseOptionType ShelfCapacity = new BookcaseOptionType(2, "Shelf capacity");

        internal static readonly IReadOnlyList<BookcaseOptionType> AvailableTypes = typeof(BookcaseOptionType)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(s => s.GetValue(s) as BookcaseOptionType)
            .ToList();

        [JsonConstructor]
        protected BookcaseOptionType(int value, string name)
            : base(value, name)
        {
        }

        public static BookcaseOptionType Get(int optionId) =>
            AvailableTypes.SingleOrDefault(p => p.Value == optionId);

        public static bool Has(int optionId) => Get(optionId) != null;
    }
}