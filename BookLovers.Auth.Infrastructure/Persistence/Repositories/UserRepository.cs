using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Infrastructure.Persistence.Repositories
{
    internal class UserRepository : IUserRepository, IRepository<User>
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _contextAccessor;

        protected UserRepository()
        {
        }

        public UserRepository(
            AuthContext context,
            IMapper mapper,
            ReadContextAccessor contextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        async Task<User> IRepository<User>.GetAsync(Guid aggregateGuid)
        {
            var readModel = await _context.Users
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            var user = _mapper.Map<User>(readModel);

            return user;
        }

        public async Task CommitChangesAsync(User aggregate)
        {
            var readModel = await _context.Users
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            var roles = await GetUserRoles(aggregate);

            if (readModel == null)
            {
                readModel = await CreateNewReadModel(aggregate, roles);
                _contextAccessor.AddReadModelId(aggregate.Guid, readModel.Id);
            }
            else
            {
                readModel.Account.Email = aggregate.Account.Email.Value;
                readModel.Account.AccountCreateDate = aggregate.Account.AccountDetails.AccountCreateDate;
                readModel.Account.HasBeenBlockedPreviously = aggregate.Account.AccountDetails.HasBeenBlockedPreviously;
                readModel.Account.Hash = aggregate.Account.AccountSecurity.Hash;
                readModel.Account.Salt = aggregate.Account.AccountSecurity.Salt;
                readModel.Account.IsBlocked = aggregate.Account.AccountSecurity.IsBlocked;
                readModel.Account.IsAccountConfirmed = aggregate.Account.AccountConfirmation.IsConfirmed;
                readModel.Account.ConfirmationDate = aggregate.Account.AccountConfirmation.ConfirmationDate;
                readModel.Status = aggregate.Status;
                readModel.Roles = roles;

                _context.Accounts.AddOrUpdate(p => p.Id, readModel.Account);
                _context.Users.AddOrUpdate(p => p.Id, readModel);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserByNickNameAsync(string nickname)
        {
            var readModel = await _context.Users
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.UserName == nickname);

            var user = _mapper.Map<User>(readModel);

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var readModel = await _context.Users
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Account.Email == email);

            var mapped = _mapper.Map<User>(readModel);

            return mapped;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var readModel = await _context.Users
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Id == userId);

            return _mapper.Map<User>(readModel);
        }

        private async Task<UserReadModel> CreateNewReadModel(
            User aggregate,
            List<UserRoleReadModel> roles)
        {
            var readModel = new UserReadModel()
            {
                Account = _mapper.Map<AccountReadModel>(aggregate.Account),
                Guid = aggregate.Guid,
                UserName = aggregate.UserName.Value,
                Roles = roles,
                Status = aggregate.Status
            };

            _context.Users.AddOrUpdate(p => p.Id, readModel);
            var num = await _context.SaveChangesAsync();

            return readModel;
        }

        private async Task<List<UserRoleReadModel>> GetUserRoles(User aggregate)
        {
            var roles = new List<UserRoleReadModel>();

            foreach (var role in aggregate.Roles)
            {
                var roleRm = await _context.Roles
                    .SingleAsync(p => p.Value == role.Role.Value);

                roles.Add(roleRm);
            }

            return roles;
        }
    }
}