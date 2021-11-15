using System;
using System.Threading.Tasks;

namespace BookLovers.Publication.Application.Contracts
{
    public interface IResourceSaver
    {
        Task SaveResourceAsync(Guid resourceGuid, ResourceInfo resourceInfo);
    }
}