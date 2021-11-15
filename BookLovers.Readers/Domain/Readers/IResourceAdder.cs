namespace BookLovers.Readers.Domain.Readers
{
    public interface IResourceAdder
    {
        AddedResourceType AddedResourceType { get; }

        void AddResource(Reader reader, IAddedResource addedResource);
    }
}