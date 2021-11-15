using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Entity;
using BookLovers.Bookcases.Domain.ShelfCategories;
using Newtonsoft.Json;

namespace BookLovers.Bookcases.Domain
{
    public class Shelf : IEntityObject
    {
        public Guid Guid { get; private set; }

        public ShelfDetails ShelfDetails { get; private set; }

        [JsonProperty] internal List<Guid> Books { get; set; } = new List<Guid>();

        private Shelf()
        {
        }

        protected Shelf(Guid shelfGuid, ShelfDetails details)
        {
            Guid = shelfGuid;
            ShelfDetails = details;
        }

        protected Shelf(Guid shelfGuid, string shelfName)
        {
            Guid = shelfGuid;
            ShelfDetails = new ShelfDetails(shelfName, ShelfDetails.Category);
        }

        protected Shelf(Guid shelfGuid, ShelfDetails shelfDetails, List<Guid> books)
        {
            Guid = shelfGuid;
            ShelfDetails = shelfDetails;
            Books = books;
        }

        public static Shelf CreateCustomShelf(Guid shelfGuid, string shelfName) =>
            new Shelf(shelfGuid, new ShelfDetails(shelfName, ShelfCategory.Custom));

        internal static Shelf CreateCoreShelf(
            Guid shelfGuid,
            string shelfName,
            ShelfCategory shelfCategory)
        {
            return new Shelf(shelfGuid, new ShelfDetails(shelfName, shelfCategory));
        }

        internal void ChangeShelfName(string shelfName) =>
            ShelfDetails = new ShelfDetails(shelfName, ShelfDetails.Category);

        public IList<Guid> GetPublications() => Books.ToList();

        public override bool Equals(object obj)
        {
            if (!(obj is Shelf shelf))
                return false;

            return Guid == shelf.Guid || ShelfDetails == shelf.ShelfDetails;
        }

        public override int GetHashCode()
        {
            var hash = 17;

            hash = (hash * 23) + this.Guid.GetHashCode();
            hash = (hash * 23) + this.Books.GetHashCode();
            hash = (hash * 23) + this.ShelfDetails.GetHashCode();

            return hash;
        }
    }
}