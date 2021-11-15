using System.IO;
using System.Linq;
using System.Web;

namespace BookLovers.Base.Infrastructure.Validation
{
    public static class ImageValidator
    {
        private static readonly string[] ValidMimeTypes = new string[5]
        {
            "image/jpg",
            "image/jpeg",
            "image/gif",
            "image/png",
            "image/pjpeg"
        };

        private static readonly string[] ValidFileExtensions = new string[5]
        {
            ".jpg",
            ".png",
            ".gif",
            "jpeg",
            ".svg"
        };

        public static bool CheckFileExtension(HttpPostedFileBase image)
        {
            var extension = Path.GetExtension(image.FileName.ToLower());

            return ValidFileExtensions.Contains(extension);
        }

        public static bool CheckMimeType(HttpPostedFileBase image)
        {
            return ValidMimeTypes.Contains(image.ContentType.ToLower());
        }
    }
}