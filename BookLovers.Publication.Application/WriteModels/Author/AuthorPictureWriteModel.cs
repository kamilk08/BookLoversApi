namespace BookLovers.Publication.Application.WriteModels.Author
{
    public class AuthorPictureWriteModel
    {
        public string AuthorImage { get; set; }

        public string FileName { get; set; }

        public bool HasImage => this.AuthorImage.Length > 0;
    }
}