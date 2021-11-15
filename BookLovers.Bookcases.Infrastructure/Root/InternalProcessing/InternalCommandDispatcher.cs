using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root.InternalProcessing
{
    internal class InternalCommandDispatcher : IInternalCommandDispatcher
    {
        public async Task SendInternalCommandAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            var commandHandlerType = typeof(ICommandHandler<>)
                .MakeGenericType(command.GetType());

            var implementation = CompositionRoot.Kernel.Get(commandHandlerType);

            await (Task) commandHandlerType.GetMethod("HandleAsync")
                .Invoke(implementation, new object[] { command });
        }
    }
}