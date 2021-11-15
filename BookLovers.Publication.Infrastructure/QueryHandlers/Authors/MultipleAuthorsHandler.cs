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
    internal class MultipleAuthorsHandler : IQueryHandler<MultipleAuthorsQuery, IList<AuthorDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public MultipleAuthorsHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<IList<AuthorDto>> HandleAsync(MultipleAuthorsQuery query)
        {
            var authors = await this._context.Authors
                .Include(p => p.Quotes)
                .Include(p => p.AuthorBooks)
                .Include(p => p.AuthorFollowers.Select(s => s.FollowedBy))
                .Include(p => p.SubCategories)
                .Include(p => p.AddedBy).AsNoTracking()
                .ActiveRecords().Where(p => query.Ids.Contains(p.Id))
                .ToListAsync();

            return this._mapper.Map<IList<AuthorReadModel>, IList<AuthorDto>>(authors);
        }
    }
}