using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.EventHandlers
{
    public class AvatarChangedHandler : IInfrastructureEventHandler<AvatarChanged>
    {
        private readonly ReadersContext _readersContext;
        private readonly IMapper _mapper;

        public AvatarChangedHandler(ReadersContext readersContext, IMapper mapper)
        {
            this._readersContext = readersContext;
            this._mapper = mapper;
        }

        public async Task HandleAsync(AvatarChanged @event)
        {
            var avatarQuery = this._readersContext.Avatars.Where(p => p.ReaderGuid == @event.ReaderGuid).FutureValue();
            var readerQuery = this._readersContext.Readers.Where(p => p.Guid == @event.ReaderGuid).FutureValue();

            var avatar = await avatarQuery.ValueAsync();
            var readerReadModel = await readerQuery.ValueAsync();

            if (avatar == null)
            {
                avatar = this._mapper.Map<AvatarChanged, AvatarReadModel>(@event);
                avatar.ReaderId = readerReadModel.ReaderId;
            }
            else
            {
                avatar.AvatarUrl = @event.AvatarUrl;
                avatar.FileName = @event.FileName;
                avatar.MimeType = @event.MimeType;
            }

            this._readersContext.Avatars.AddOrUpdate(p => p.Id, avatar);

            await this._readersContext.SaveChangesAsync();
        }
    }
}