using System.Web.Http;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class NotFoundController : ApiController
    {
        public IHttpActionResult NotFound(string uri)
        {
            return this.NotFoundResult();
        }

        public IHttpActionResult Put(string uri)
        {
            return this.NotFoundResult();
        }

        public IHttpActionResult Delete(string uri)
        {
            return this.NotFoundResult();
        }

        public IHttpActionResult Get(string uri)
        {
            return this.NotFoundResult();
        }
    }
}