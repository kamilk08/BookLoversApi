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
    internal class MultipleAuthorsByGuidHandler :
        IQueryHandler<MultipleAuthorsByGuidQuery, IList<AuthorDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public MultipleAuthorsByGuidHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<IList<AuthorDto>> HandleAsync(
            MultipleAuthorsByGuidQuery query)
        {
            var authors = await this._context.Authors
                .Include(p => p.Quotes).Include(p => p.AuthorBooks)
                .Include(p => p.AuthorFollowers.Select(s => s.FollowedBy))
                .Include(p => p.SubCategories)
                .Include(p => p.AddedBy)
                .AsNoTracking()
                .ActiveRecords()
                .Where(p => query.Guides.Contains(p.Guid))
                .ToListAsync();

            return this._mapper.Map<IList<AuthorReadModel>, IList<AuthorDto>>(authors);
        }
    }
}