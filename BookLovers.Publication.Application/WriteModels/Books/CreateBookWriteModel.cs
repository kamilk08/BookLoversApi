namespace BookLovers.Publication.Application.WriteModels.Books
{
    public class CreateBookWriteModel
    {
        public BookWriteModel BookWriteModel { get; set; }

        public BookPictureWriteModel PictureWriteModel { get; set; }

        public int BookId { get; internal set; }
    }
}