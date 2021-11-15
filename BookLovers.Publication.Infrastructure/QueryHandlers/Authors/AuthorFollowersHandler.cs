using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using BookLovers.Publication.Infrastructure.Queries.Authors;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Authors
{
    internal class AuthorFollowersHandler :
        IQueryHandler<AuthorFollowersQuery, List<AuthorFollowerDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public AuthorFollowersHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<AuthorFollowerDto>> HandleAsync(
            AuthorFollowersQuery query)
        {
            var followers = await this._context.Authors
                .Include(p => p.AuthorFollowers).AsNoTracking()
                .ActiveRecords()
                .WithId(query.AuthorId).SelectMany(sm => sm.AuthorFollowers)
                .ToListAsync();

            return this._mapper.Map<List<AuthorFollowerReadModel>, List<AuthorFollowerDto>>(followers);
        }
    }
}