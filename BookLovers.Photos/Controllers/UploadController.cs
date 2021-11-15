using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using BookLovers.Photos.Models;
using BookLovers.Photos.Models.Services;

namespace BookLovers.Photos.Controllers
{
    public class UploadController : Controller
    {
        private readonly IDictionary<ProviderType, IFilePathFactory> _pathProviders;

        public UploadController(
            IDictionary<ProviderType, IFilePathFactory> pathProviders)
        {
            this._pathProviders = pathProviders;
        }

        [HttpGet]
        [Route("upload/books/{bookId}", Name = "Cover")]
        public async Task<ActionResult> Cover(int bookId)
        {
            var pathResult = await _pathProviders[ProviderType.BookCoverProvider].GetPath(bookId);
            if (string.IsNullOrEmpty(pathResult.Url))
                return new EmptyResult();

            return File(pathResult.Url, pathResult.ContentType);
        }

        [HttpGet]
        [Route("upload/authors/{authorId}", Name = "AuthorImage")]
        public async Task<ActionResult> AuthorImage(int authorId)
        {
            var pathResult = await _pathProviders[ProviderType.AuthorImageProvider].GetPath(authorId);
            if (string.IsNullOrEmpty(pathResult.Url))
                return new EmptyResult();

            return File(pathResult.Url, pathResult.ContentType);
        }

        [HttpGet]
        [Route("upload/avatars/{userId}", Name = "Avatar")]
        public async Task<ActionResult> Avatar(int userId)
        {
            var pathResult = await _pathProviders[ProviderType.AvatarImageProvider].GetPath(userId);
            if (string.IsNullOrEmpty(pathResult.Url))
                return new EmptyResult();

            return File(pathResult.Url, pathResult.ContentType);
        }
    }
}