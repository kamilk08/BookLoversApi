using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books
{
    public class CategoryReadModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public IList<SubCategoryReadModel> SubCategories { get; set; }

        public CategoryReadModel()
        {
            this.SubCategories = new List<SubCategoryReadModel>();
        }
    }
}