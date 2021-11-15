using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorBasicsProjection : IProjectionHandler<AuthorBasicsChanged>, IProjectionHandler
    {
        private readonly PublicationsContext _context;

        public AuthorBasicsProjection(PublicationsContext context)
        {
            this._context = context;
        }

        public void Handle(AuthorBasicsChanged @event)
        {
            this._context.Authors.Where(p => p.Guid == @event.AggregateGuid)
                .Update(p => new AuthorReadModel
                {
                    FirstName = @event.FirstName,
                    SecondName = @event.SecondName,
                    Sex = @event.SexId,
                    FullName = @event.FullName
                });
        }
    }
}