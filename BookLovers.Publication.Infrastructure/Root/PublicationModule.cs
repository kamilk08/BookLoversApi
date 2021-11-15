using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root
{
    public class PublicationModule : IModule<PublicationModule>, IModule
    {
        Task<QueryResult<TQuery, TResult>> IModule<PublicationModule>.ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
        {
            return CompositionRoot.Kernel.Get<ModuleLoggingDecorator>()
                .ExecuteQueryAsync<TQuery, TResult>(query);
        }

        Task<ICommandValidationResult> IModule<PublicationModule>.SendCommandAsync<TCommand>(
            TCommand command)
        {
            return CompositionRoot.Kernel.Get<ModuleLoggingDecorator>()
                .SendCommandAsync(command);
        }
    }
}