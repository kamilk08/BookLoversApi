using System.Data.Entity;
using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Shelf;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class CoreShelfCreatedProjection :
        IProjectionHandler<CoreShelfCreated>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public CoreShelfCreatedProjection(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle(CoreShelfCreated @event)
        {
            var bookcase = _context.Bookcases.Include(p => p.Shelves)
                .Single(p => p.Guid == @event.AggregateGuid);

            var shelf = _mapper.Map<CoreShelfCreated, ShelfReadModel>(@event);

            shelf.Bookcase = bookcase;
            bookcase.Shelves.Add(shelf);

            _context.Shelves.Add(shelf);

            _context.SaveChanges();
        }
    }
}