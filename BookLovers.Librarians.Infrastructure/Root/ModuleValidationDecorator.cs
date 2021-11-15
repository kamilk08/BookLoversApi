using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Root.Validation;
using Ninject;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Root
{
    internal class ModuleValidationDecorator :
        BaseValidationModule,
        IModule<LibrarianModule>,
        IModule,
        IValidationDecorator<LibrarianModule>
    {
        public ModuleValidationDecorator(ValidatorFactory validatorFactory)
            : base(validatorFactory)
        {
        }

        public async Task<QueryResult<TQuery, TResult>> ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            var validationResult = ValidateQuery<TQuery, TResult>(query);

            if (validationResult.HasErrors)
                return validationResult;

            var result = await CompositionRoot.Kernel.Get<IQueryHandler<TQuery, TResult>>().HandleAsync(query);

            return validationResult.HasErrors
                ? validationResult
                : QueryResult<TQuery, TResult>.ValidQuery(query, result);
        }

        public async Task<ICommandValidationResult> SendCommandAsync<TCommand>(
            TCommand command)
            where TCommand : class, ICommand
        {
            var commandHandler = CompositionRoot.Kernel.Get<ICommandHandler<TCommand>>();

            if (command is IInternalCommand)
            {
                await commandHandler.HandleAsync(command);
                return CommandValidationResult.SuccessResult();
            }

            var validationResult = this.ValidateCommand(command);

            if (validationResult.HasErrors)
                return validationResult;

            await commandHandler.HandleAsync(command);

            return CommandValidationResult.SuccessResult();
        }
    }
}