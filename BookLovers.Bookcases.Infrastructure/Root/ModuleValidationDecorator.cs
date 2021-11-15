using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Root.Validation;
using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root
{
    public class ModuleValidationDecorator :
        BaseValidationModule,
        IModule<BookcaseModule>,
        IModule,
        IValidationDecorator<BookcaseModule>
    {
        private readonly ValidatorFactory _validatorFactory;
        private readonly IModule<BookcaseModule> _module;

        public ModuleValidationDecorator(
            ValidatorFactory validatorFactory,
            IModule<BookcaseModule> module)
            : base(validatorFactory)
        {
            _validatorFactory = validatorFactory;
            _module = module;
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
            var validationDecorator = this;
            var commandHandler = CompositionRoot.Kernel.Get<ICommandHandler<TCommand>>();

            if (command is IInternalCommand)
            {
                await commandHandler.HandleAsync(command);

                return CommandValidationResult.SuccessResult();
            }

            var validationResult = validationDecorator.ValidateCommand(command);

            if (validationResult.HasErrors)
                return validationResult;

            await commandHandler.HandleAsync(command);

            return CommandValidationResult.SuccessResult();
        }
    }
}