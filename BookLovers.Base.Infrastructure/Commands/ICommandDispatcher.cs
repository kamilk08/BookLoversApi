using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Commands
{
    public interface ICommandDispatcher
    {
        Task<ICommandValidationResult> DispatchAsync<T>(T command)
            where T : class, ICommand;
    }
}