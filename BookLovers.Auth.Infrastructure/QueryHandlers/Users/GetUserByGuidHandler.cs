using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Auth.Infrastructure.Dtos.Users;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Users
{
    internal class GetUserByGuidHandler : IQueryHandler<GetUserByGuidQuery, UserDto>
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public GetUserByGuidHandler(AuthContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> HandleAsync(GetUserByGuidQuery query)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(p => p.Account)
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Guid == query.Guid);

            var userDto = _mapper.Map<UserReadModel, UserDto>(user);

            return userDto;
        }
    }
}