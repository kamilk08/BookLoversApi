namespace BookLovers.Photos.Models
{
    public class PathResult
    {
        public string Url { get; }

        public string ContentType { get; }

        public PathResult(string url, string contentType)
        {
            this.Url = url;
            this.ContentType = contentType;
        }
    }
}