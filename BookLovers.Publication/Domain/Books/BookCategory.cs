using BookLovers.Base.Domain.ValueObject;
using BookLovers.Shared.Categories;

namespace BookLovers.Publication.Domain.Books
{
    public class BookCategory : ValueObject<BookCategory>
    {
        public Category Category { get; }

        public SubCategory SubCategory { get; }

        private BookCategory()
        {
        }

        public BookCategory(Category category, SubCategory subCategory)
        {
            this.Category = category;
            this.SubCategory = subCategory;
        }

        public BookCategory(int categoryId, int subCategoryId)
        {
            this.Category = CategoryList.Get(categoryId);
            this.SubCategory = SubCategoryList.Get(subCategoryId, categoryId);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Category.GetHashCode();
            hash = (hash * 23) + this.SubCategory.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(BookCategory obj)
        {
            return this.Category == obj.Category && this.SubCategory == obj.SubCategory;
        }
    }
}