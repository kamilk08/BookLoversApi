using System.Data.Entity.Migrations;
using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Profile;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Profiles
{
    internal class SocialProfileCreatedProjection :
        IProjectionHandler<ProfileCreated>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public SocialProfileCreatedProjection(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public void Handle(ProfileCreated @event)
        {
            var readerQuery = this._context.Readers.Where(p => p.Guid == @event.ReaderGuid).FutureValue();
            var sexesQuery = this._context.Sexes.Where(p => p.Id == @event.Sex).FutureValue();

            var reader = readerQuery.Value;
            var sex = sexesQuery.Value;

            var entity = new ProfileReadModel()
            {
                Reader = reader,
                JoinedAt = @event.JoinedAt,
                Guid = @event.AggregateGuid,
                Sex = @event.Sex,
                SexName = sex.Name,
                Status = @event.Status,
                CurrentRole = @event.CurrentRole
            };

            this._context.Profiles.Add(entity);

            this._context.Readers.AddOrUpdate(p => p.Id, reader);

            this._context.SaveChanges();
        }
    }
}