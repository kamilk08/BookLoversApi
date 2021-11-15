using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Shelf;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using BookLovers.Bookcases.Infrastructure.Root;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class CustomShelfCreatedProjection :
        IProjectionHandler<CustomShelfCreated>,
        IProjectionHandler
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _contextAccessor;

        public CustomShelfCreatedProjection(
            BookcaseContext context,
            IMapper mapper,
            ReadContextAccessor contextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public void Handle(CustomShelfCreated @event)
        {
            var bookcaseReadModel = _context.Bookcases
                .Single(p => p.Guid == @event.AggregateGuid);
            var shelfReadModel = _mapper.Map<ShelfReadModel>(@event);

            shelfReadModel.Bookcase = bookcaseReadModel;
            bookcaseReadModel.Shelves.Add(shelfReadModel);

            _context.SaveChanges();

            _contextAccessor.AddReadModelId(@event.ShelfGuid, shelfReadModel.Id);
        }
    }
}