using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root
{
    public class BookcaseModule : IModule<BookcaseModule>, IModule
    {
        async Task<QueryResult<TQuery, TResult>> IModule<BookcaseModule>.ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
        {
            return await CompositionRoot.Kernel.Get<ModuleLoggingDecorator>().ExecuteQueryAsync<TQuery, TResult>(query);
        }

        async Task<ICommandValidationResult> IModule<BookcaseModule>.SendCommandAsync<TCommand>(
            TCommand command)
        {
            return await CompositionRoot.Kernel.Get<ModuleLoggingDecorator>().SendCommandAsync(command);
        }
    }
}