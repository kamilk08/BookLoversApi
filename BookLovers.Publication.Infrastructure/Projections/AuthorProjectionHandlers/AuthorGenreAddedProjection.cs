using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorGenreAddedProjection :
        IProjectionHandler<AuthorGenreAdded>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorGenreAddedProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorGenreAdded @event)
        {
            var subCategory = this._context.SubCategories.Include(p => p.Category)
                .Single(p => p.Id == @event.SubCategoryId);
            var author = this._context.Authors.Include(p => p.SubCategories)
                .Single(p => p.Guid == @event.AggregateGuid);

            author.SubCategories.Add(subCategory);

            this._context.Authors.AddOrUpdate(p => p.Id, author);

            this._context.SaveChanges();
        }
    }
}