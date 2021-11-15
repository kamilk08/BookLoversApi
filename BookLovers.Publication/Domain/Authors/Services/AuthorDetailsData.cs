using BookLovers.Shared;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public class AuthorDetailsData
    {
        public LifeLength LifeLength { get; }

        public string BirthPlace { get; }

        internal AuthorDetailsData(LifeLength lifeLength, string birthPlace)
        {
            this.LifeLength = lifeLength;
            this.BirthPlace = birthPlace;
        }
    }
}