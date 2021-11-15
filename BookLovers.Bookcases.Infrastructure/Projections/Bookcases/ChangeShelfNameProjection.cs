using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Shelf;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class ChangeShelfNameProjection : IProjectionHandler<ShelfNameChanged>, IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public ChangeShelfNameProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(ShelfNameChanged @event)
        {
            _context.Shelves
                .Where(p => p.Guid == @event.ShelfGuid)
                .Update(p => new ShelfReadModel
                {
                    ShelfName = @event.ShelfName
                });
        }
    }
}