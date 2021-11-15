using System.Threading.Tasks;

namespace BookLovers.Publication.Domain.Authors.Services.Editors
{
    public interface IAuthorEditor
    {
        Task Edit(Author author, AuthorData authorDto);
    }
}