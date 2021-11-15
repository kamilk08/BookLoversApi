using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Bookcases.Domain.ShelfCategories
{
    public static class ShelfCategoryList
    {
        private static IReadOnlyCollection<ShelfCategory> _categories =
            typeof(ShelfCategory)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as ShelfCategory).ToList();

        public static ShelfCategory Get(int shelfCategory) =>
            _categories.Single(p => p.Value == shelfCategory);
    }
}