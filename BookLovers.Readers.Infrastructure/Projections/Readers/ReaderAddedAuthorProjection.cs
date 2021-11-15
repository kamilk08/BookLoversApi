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
    internal class ReaderAddedAuthorProjection :
        IProjectionHandler<ReaderAddedAuthor>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderAddedAuthorProjection(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public void Handle(ReaderAddedAuthor @event)
        {
            var reader = this._context.Readers.Include(p => p.AddedResources)
                .Single(p => p.Guid == @event.AggregateGuid);

            var resource = new AddedResourceReadModel()
            {
                ResourceGuid = @event.AuthorGuid
            };

            var author = new AuthorReadModel()
            {
                AuthorGuid = @event.AuthorGuid,
                AuthorId = @event.AuthorId
            };

            reader.AddedResources.Add(resource);

            this._context.Readers.AddOrUpdate(p => p.Id, reader);

            this._context.AddedResources.AddOrUpdate(p => p.ResourceGuid, resource);

            this._context.Authors.AddOrUpdate(p => p.AuthorGuid, author);

            this._context.SaveChanges();
        }
    }
}