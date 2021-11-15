namespace BookLovers.Photos.Models.Avatars
{
    public class AvatarReadModel
    {
        public int Id { get; set; }

        public int ReaderId { get; set; }

        public string AvatarUrl { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }
    }
}