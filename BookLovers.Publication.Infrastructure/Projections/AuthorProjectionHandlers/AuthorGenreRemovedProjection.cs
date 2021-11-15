using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorGenreRemovedProjection :
        IProjectionHandler<AuthorGenreRemoved>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorGenreRemovedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorGenreRemoved @event)
        {
            var author = this._context.Authors.Include(p => p.SubCategories)
                .Single(p => p.Guid == @event.AggregateGuid);
            var subCategory = author.SubCategories.Single(p => p.Id == @event.SubCategoryId);

            author.SubCategories.Remove(subCategory);

            this._context.Authors.AddOrUpdate(p => p.Id, author);

            this._context.SaveChanges();
        }
    }
}