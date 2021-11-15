namespace BookLovers.Publication.Application.WriteModels.Books
{
    public class BookPictureWriteModel
    {
        public string Cover { get; set; }

        public string FileName { get; set; }

        public bool HasImage => !string.IsNullOrEmpty(this.Cover);
    }
}