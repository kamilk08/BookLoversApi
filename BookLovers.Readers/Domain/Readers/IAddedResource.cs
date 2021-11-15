using System;

namespace BookLovers.Readers.Domain.Readers
{
    public interface IAddedResource
    {
        Guid ResourceGuid { get; }

        AddedResourceType AddedResourceType { get; }

        DateTime AddedAt { get; }
    }
}