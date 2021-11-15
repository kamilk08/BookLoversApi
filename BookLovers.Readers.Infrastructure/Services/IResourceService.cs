using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Contracts;

namespace BookLovers.Readers.Infrastructure.Services
{
    public interface IResourceService
    {
        ResourceType ResourceType { get; }

        ResourceOwner ResourceOwner { get; }

        Task SaveResourceAsync(IResource resource, Guid resourceGuid);
    }
}