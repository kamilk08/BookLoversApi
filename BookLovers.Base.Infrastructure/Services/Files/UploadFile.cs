using System.IO;
using System.Linq;
using System.Web;

namespace BookLovers.Base.Infrastructure.Services.Files
{
    public class UploadFile : HttpPostedFileBase, IResource
    {
        public override int ContentLength { get; }

        public override string ContentType { get; }

        public override string FileName { get; }

        public override Stream InputStream { get; }

        public UploadFile(HttpPostedFile postedFile)
        {
            ContentLength = postedFile.ContentLength;
            ContentType = postedFile.ContentType;
            FileName = postedFile.FileName;
            InputStream = postedFile.InputStream;
        }

        public UploadFile()
        {
            ContentLength = 0;
            ContentType = string.Empty;
            FileName = string.Empty;
            InputStream = Stream.Null;
        }

        public UploadFile(int contentLength, string contentType, string fileName, Stream inputStream)
        {
            ContentLength = contentLength;
            ContentType = contentType;
            FileName = fileName;
            InputStream = inputStream;
        }

        public UploadFile(byte[] content, string fileName)
        {
            InputStream = new MemoryStream(content);
            FileName = fileName;
            ContentType = fileName.Split('.').Last();
            ContentLength = content.Length;
        }

        public bool HasContent() => (uint) ContentLength > 0;

        public override string ToString() => base.ToString();

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();
    }
}