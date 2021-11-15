using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class ReaderAddedBookProjection : IProjectionHandler<ReaderAddedBook>, IProjectionHandler
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderAddedBookProjection(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public void Handle(ReaderAddedBook @event)
        {
            var reader = this._context.Readers
                .Include(p => p.AddedResources)
                .Single(p => p.Guid == @event.AggregateGuid);

            var resource = new AddedResourceReadModel()
            {
                ResourceGuid = @event.BookGuid
            };

            var book = new BookReadModel()
            {
                BookGuid = @event.BookGuid,
                BookId = @event.BookId
            };

            reader.AddedResources.Add(resource);

            this._context.Readers.AddOrUpdate(p => p.Id, reader);

            this._context.Books.Add(book);

            this._context.SaveChanges();
        }
    }
}