using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Messages;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root.Jobs
{
    public class CheckNotConfirmedAccountsJob : NonConcurrentJob
    {
        public override void Execute()
        {
            var dispatcher = CompositionRoot.Kernel
                .Get<IInternalCommandDispatcher>();

            Task.Run(async () =>await dispatcher.SendInternalCommandAsync(new CheckAccountsConfirmationCommand()));
        }
    }
}