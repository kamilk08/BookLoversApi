using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Shared.Categories
{
    public static class SubCategoryList
    {
        public static readonly IReadOnlyCollection<SubCategory> SubCategories = Assembly
            .GetExecutingAssembly()
            .GetTypes().Where(p => p.IsSubclassOf(typeof(SubCategory)))
            .SelectMany(p => p.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(s => s.GetValue(s) as SubCategory)).ToList();

        public static SubCategory Get(int subcategoryId,int categoryId) =>
            SubCategories.SingleOrDefault(p => p.Value == subcategoryId && p.Category.Value==categoryId);
    }
}
