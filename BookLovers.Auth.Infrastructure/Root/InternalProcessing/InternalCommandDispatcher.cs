using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root.InternalProcessing
{
    internal class InternalCommandDispatcher : IInternalCommandDispatcher
    {
        public async Task SendInternalCommandAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            var interfaceType = typeof(ICommandHandler<>)
                .MakeGenericType(command.GetType());

            var implementation = CompositionRoot.Kernel.Get(interfaceType);

            await (Task)interfaceType.GetMethod("HandleAsync")
                .Invoke(implementation, new object[] { command });
        }
    }
}