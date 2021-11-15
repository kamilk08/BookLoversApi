using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Infrastructure.Root.Validation;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root
{
    public class ReadersModule : IModule<ReadersModule>, IModule
    {
        private readonly ValidatorFactory _validatorFactory;

        public ReadersModule() => this._validatorFactory = CompositionRoot.Kernel.Get<ValidatorFactory>();

        Task<QueryResult<TQuery, TResult>> IModule<ReadersModule>.ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
        {
            return CompositionRoot.Kernel.Get<ModuleLoggingDecorator>().ExecuteQueryAsync<TQuery, TResult>(query);
        }

        Task<ICommandValidationResult> IModule<ReadersModule>.SendCommandAsync<TCommand>(
            TCommand command)
        {
            return CompositionRoot.Kernel.Get<ModuleLoggingDecorator>().SendCommandAsync(command);
        }
    }
}