using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;

namespace BookLovers.Publication.Domain.Authors.Services.Editors
{
    public class AuthorEditService : IDomainService<Author>
    {
        private readonly List<IAuthorEditor> _authorEditors;

        public AuthorEditService(List<IAuthorEditor> authorEditors)
        {
            this._authorEditors = authorEditors;
        }

        public Task EditAuthor(Author author, AuthorData dto)
        {
            return Task.WhenAll(this._authorEditors.Select(editor => editor
                .Edit(author, dto)));
        }
    }
}