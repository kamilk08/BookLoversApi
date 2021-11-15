using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers;

namespace BookLovers.Readers.Infrastructure.QueryHandlers
{
    internal class ReaderAvatarQueryHandler : IQueryHandler<ReaderAvatarQuery, Tuple<string, string>>
    {
        private readonly ReadersContext _context;

        public ReaderAvatarQueryHandler(ReadersContext context)
        {
            _context = context;
        }

        public async Task<Tuple<string, string>> HandleAsync(ReaderAvatarQuery query)
        {
            var readModel = await _context.Avatars
                .Where(p => p.ReaderId == query.ReaderId)
                .SingleOrDefaultAsync();

            var result = new Tuple<string, string>(readModel.FileName, readModel.MimeType);

            return result;
        }
    }
}