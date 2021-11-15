using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Root.Validation;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root
{
    internal class ModuleValidationDecorator :
        BaseValidationModule,
        IModule<AuthModule>,
        IModule,
        IValidationDecorator<AuthModule>
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

            return QueryResult<TQuery, TResult>.ValidQuery(query, result);
        }

        public async Task<ICommandValidationResult> SendCommandAsync<TCommand>(
            TCommand command)
            where TCommand : class, ICommand
        {
            var handler = CompositionRoot.Kernel.Get<ICommandHandler<TCommand>>();

            if (command is IInternalCommand)
            {
                await handler.HandleAsync(command);
                return CommandValidationResult.SuccessResult();
            }

            var validationResult = ValidateCommand(command);
            if (validationResult.HasErrors)
                return validationResult;

            await handler.HandleAsync(command);

            return CommandValidationResult.SuccessResult();
        }
    }
}