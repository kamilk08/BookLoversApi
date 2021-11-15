using System;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public interface IAuthorUniquenessChecker
    {
        bool IsUnique(Guid guid);
    }
}