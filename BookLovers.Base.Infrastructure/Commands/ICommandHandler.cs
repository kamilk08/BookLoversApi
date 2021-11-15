using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Commands
{
    public interface ICommandHandler<in T>
        where T : class, ICommand
    {
        Task HandleAsync(T command);
    }
}