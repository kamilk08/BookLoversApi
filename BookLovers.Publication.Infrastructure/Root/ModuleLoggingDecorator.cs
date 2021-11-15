using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using Serilog;

namespace BookLovers.Publication.Infrastructure.Root
{
    public class ModuleLoggingDecorator : IModule<PublicationModule>, IModule
    {
        private readonly IModule<PublicationModule> _module;
        private readonly ILogger _logger;

        public ModuleLoggingDecorator(IModule<PublicationModule> module, ILogger logger)
        {
            _module = module;
            _logger = logger;
        }

        public Task<QueryResult<TQuery, TResult>> ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            try
            {
                _logger.Information("Publications module executing query of type " + query?.GetType().Name);

                var task = _module.ExecuteQueryAsync<TQuery, TResult>(query);

                _logger.Information("Query of type " + query?.GetType().Name + " was executed with success.");

                return task;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while processing query of type " + query?.GetType().Name + ".");

                throw;
            }
        }

        public async Task<ICommandValidationResult> SendCommandAsync<TCommand>(
            TCommand command)
            where TCommand : class, ICommand
        {
            ICommandValidationResult validationResult;
            try
            {
                _logger.Information("Module sending command of type " + command.GetType().Name);

                validationResult = await _module.SendCommandAsync(command);

                _logger.Information(
                    $"Command of type {command.GetType()} was processed with result {validationResult.Status}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Command " + command.GetType().Name + " processing failure.");

                throw;
            }

            return validationResult;
        }
    }
}