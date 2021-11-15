using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Ratings.Infrastructure.Root
{
    public class RatingsModule : IModule<RatingsModule>, IModule
    {
        async Task<QueryResult<TQuery, TResult>> IModule<RatingsModule>.ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
        {
            return await CompositionRoot.Kernel.Get<ModuleLoggingDecorator>().ExecuteQueryAsync<TQuery, TResult>(query);
        }

        async Task<ICommandValidationResult> IModule<RatingsModule>.SendCommandAsync<TCommand>(
            TCommand command)
        {
            return await CompositionRoot.Kernel.Get<ModuleValidationDecorator>().SendCommandAsync(command);
        }
    }
}