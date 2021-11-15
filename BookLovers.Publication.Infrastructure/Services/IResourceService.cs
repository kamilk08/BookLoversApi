using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Contracts;

namespace BookLovers.Publication.Infrastructure.Services
{
    public interface IResourceService
    {
        ResourceType ResourceType { get; }

        ResourceOwner ResourceOwner { get; }

        Task SaveResourceAsync(IResource resource, Guid resourceGuid);
    }
}