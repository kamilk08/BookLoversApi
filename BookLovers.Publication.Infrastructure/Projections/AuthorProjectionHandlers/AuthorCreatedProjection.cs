using System.Data.Entity.Migrations;
using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using BookLovers.Publication.Infrastructure.Services;

namespace BookLovers.Publication.Infrastructure.Projections.AuthorProjectionHandlers
{
    internal class AuthorCreatedProjection : IProjectionHandler<AuthorCreated>, IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly ReadContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public AuthorCreatedProjection(
            PublicationsContext context,
            ReadContextAccessor contextAccessor,
            IMapper mapper)
        {
            this._context = context;
            this._contextAccessor = contextAccessor;
            this._mapper = mapper;
        }

        public void Handle(AuthorCreated @event)
        {
            var reader = this._context.Readers.Single(p => p.ReaderGuid == @event.ReaderGuid);
            var author = this._mapper.Map<AuthorCreated, AuthorReadModel>(@event);

            author.AddedBy = reader;

            this._context.Authors.AddOrUpdate(p => p.Guid, author);

            this._context.SaveChanges();

            this._contextAccessor.AddReadModelId(@event.AggregateGuid, author.Id);
        }
    }
}