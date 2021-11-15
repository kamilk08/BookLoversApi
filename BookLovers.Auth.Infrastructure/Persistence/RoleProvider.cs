using System.Linq;
using AutoMapper;
using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Users;

namespace BookLovers.Auth.Infrastructure.Persistence
{
    public class RoleProvider : IRoleProvider
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public RoleProvider(AuthContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserRole GetRole(Role role)
        {
            var readModel = _context.Roles.Single(p => p.Value == role.Value);

            return _mapper.Map<UserRole>(readModel);
        }
    }
}