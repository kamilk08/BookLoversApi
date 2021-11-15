using System.Data.Entity.Migrations;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.Readers
{
    internal class ReaderRegisteredProjection : IProjectionHandler<ReaderCreated>, IProjectionHandler
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderRegisteredProjection(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public void Handle(ReaderCreated @event)
        {
            var reader = this._mapper.Map<ReaderCreated, ReaderReadModel>(@event);

            this._context.Readers.AddOrUpdate(p => p.Guid, reader);

            this._context.SaveChanges();
        }
    }
}