using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root
{
    public class AuthModule : IModule<AuthModule>, IModule
    {
        Task<QueryResult<TQuery, TResult>> IModule<AuthModule>.ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
        {
            return CompositionRoot.Kernel.Get<ModuleLoggingDecorator>()
                .ExecuteQueryAsync<TQuery, TResult>(query);
        }

        Task<ICommandValidationResult> IModule<AuthModule>.SendCommandAsync<TCommand>(
            TCommand command)
        {
            return CompositionRoot.Kernel.Get<ModuleLoggingDecorator>()
                .SendCommandAsync(command);
        }
    }
}