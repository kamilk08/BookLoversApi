using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Domain.Services
{
    public interface IDomainService<T>
        where T : IRoot
    {
    }
}