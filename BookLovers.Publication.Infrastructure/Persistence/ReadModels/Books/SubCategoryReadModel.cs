namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books
{
    public class SubCategoryReadModel
    {
        public int Id { get; set; }

        public CategoryReadModel Category { get; set; }

        public int CategoryId { get; set; }

        public string SubCategoryName { get; set; }
    }
}