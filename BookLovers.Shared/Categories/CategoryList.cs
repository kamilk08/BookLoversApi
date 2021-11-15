using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Shared.Categories
{
    public static class CategoryList
    {
        public static readonly IReadOnlyCollection<Category> Categories =
            typeof(Category)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetValue(f) as Category).ToList();

        public static Category Get(int categoryId) => Categories.SingleOrDefault(p => p.Value == categoryId);

    }
}
