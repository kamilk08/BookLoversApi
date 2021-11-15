using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class CategoryDto
    {
        public byte Id { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<SubCategoryDto> SubCategories { get; set; }
    }
}