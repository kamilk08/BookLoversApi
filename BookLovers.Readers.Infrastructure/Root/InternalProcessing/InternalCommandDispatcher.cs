using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.InternalProcessing
{
    internal class InternalCommandDispatcher : IInternalCommandDispatcher
    {
        public async Task SendInternalCommandAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            var type = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            var implementation = CompositionRoot.Kernel.Get(type);

            await (Task) type.GetMethod("HandleAsync")
                .Invoke(implementation, new object[] { command });
        }
    }
}