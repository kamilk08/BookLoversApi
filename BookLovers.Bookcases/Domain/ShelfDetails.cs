using BookLovers.Base.Domain.ValueObject;
using BookLovers.Bookcases.Domain.ShelfCategories;

namespace BookLovers.Bookcases.Domain
{
    public class ShelfDetails : ValueObject<ShelfDetails>
    {
        public string ShelfName { get; }

        public ShelfCategory Category { get; }

        private ShelfDetails()
        {
        }

        public ShelfDetails(string shelfName, ShelfCategory category)
        {
            ShelfName = shelfName;
            Category = category;
        }

        public bool IsCoreShelf() => Category != ShelfCategory.Custom;

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.ShelfName.GetHashCode();
            hash = (hash * 23) + this.Category.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ShelfDetails obj)
            => ShelfName == obj.ShelfName && Category == obj.Category;
    }
}