using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Ratings.Infrastructure.Root.InternalProcessing
{
    internal class InternalCommandDispatcher : IInternalCommandDispatcher
    {
        public async Task SendInternalCommandAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            var service = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            var implementation = CompositionRoot.Kernel.Get(service);

            await (Task) service.GetMethod("HandleAsync")
                .Invoke(implementation, new object[] { command });
        }
    }
}