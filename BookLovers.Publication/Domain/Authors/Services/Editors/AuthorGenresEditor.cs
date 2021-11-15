using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Shared.Categories;

namespace BookLovers.Publication.Domain.Authors.Services.Editors
{
    public class AuthorGenresEditor : IAuthorEditor
    {
        public Task Edit(Author author, AuthorData authorDto)
        {
            var oldSequence = author.Genres.AsEnumerable();

            var newSequence = SubCategoryList.SubCategories
                .Where(p => authorDto.Genres.Any(a => p.Value == a))
                .AsEnumerable();

            Distinguisher<SubCategory>.ToRemove(oldSequence, newSequence)
                .ToList().ForEach(author.RemoveGenre);

            Distinguisher<SubCategory>.ToAdd(oldSequence, newSequence)
                .ToList().ForEach(author.AddGenre);

            return Task.CompletedTask;
        }
    }
}