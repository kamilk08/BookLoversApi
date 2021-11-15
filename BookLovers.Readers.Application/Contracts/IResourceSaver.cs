using System;
using System.Threading.Tasks;

namespace BookLovers.Readers.Application.Contracts
{
    public interface IResourceSaver
    {
        Task SaveResourceAsync(Guid resourceGuid, ResourceInfo resourceInfo);
    }
}