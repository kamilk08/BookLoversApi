using System;

namespace BookLovers.Publication.Domain.Publishers.Services
{
    public interface IPublisherUniquenessChecker
    {
        bool IsUnique(Guid guid, string name);
    }
}