using System;
using System.Collections.Generic;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.BusinessRules;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Roles
{
    public class RoleRulesCollection
    {
        private readonly Dictionary<Role, Func<User, IBusinessRule>> _roleRules =
            new Dictionary<Role, Func<User, IBusinessRule>>();

        internal RoleRulesCollection()
        {
            _roleRules.Add(Role.Librarian, user => new LibrarianRoleRules(user));
            _roleRules.Add(Role.Reader, user => new ReaderRoleRules(user));
            _roleRules.Add(Role.SuperAdmin, user => new AggregateMustBeActive(user.Status));
        }

        internal Func<User, IBusinessRule> GetRule(Role role) => _roleRules[role];

        internal void AddUserRole(Role role, IBusinessRule rule)
        {
            if (role == null || rule == null)
                throw new ArgumentNullException();

            _roleRules.Add(role, user => rule);
        }

        internal void RemoveUserRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException();

            _roleRules.Remove(role);
        }
    }
}