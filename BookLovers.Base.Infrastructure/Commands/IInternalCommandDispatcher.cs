using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Commands
{
    public interface IInternalCommandDispatcher
    {
        Task SendInternalCommandAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand;
    }
}