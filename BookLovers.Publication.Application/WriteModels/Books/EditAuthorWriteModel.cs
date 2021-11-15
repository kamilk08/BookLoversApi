using BookLovers.Publication.Application.WriteModels.Author;

namespace BookLovers.Publication.Application.WriteModels.Books
{
    public class EditAuthorWriteModel
    {
        public AuthorWriteModel AuthorWriteModel { get; set; }

        public AuthorPictureWriteModel PictureWriteModel { get; set; }
    }
}