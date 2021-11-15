using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using Ninject;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Root
{
    public class LibrarianModule : IModule<LibrarianModule>, IModule
    {
        async Task<QueryResult<TQuery, TResult>> IModule<LibrarianModule>.ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
        {
            return await CompositionRoot.Kernel.Get<ModuleLoggingDecorator>().ExecuteQueryAsync<TQuery, TResult>(query);
        }

        async Task<ICommandValidationResult> IModule<LibrarianModule>.SendCommandAsync<TCommand>(
            TCommand command)
        {
            return await CompositionRoot.Kernel.Get<ModuleLoggingDecorator>().SendCommandAsync(command);
        }
    }
}