using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using Serilog;

namespace BookLovers.Readers.Infrastructure.Root
{
    public class ModuleLoggingDecorator : IModule<ReadersModule>, IModule
    {
        private readonly IModule<ReadersModule> _module;
        private readonly ILogger _logger;

        public ModuleLoggingDecorator(IModule<ReadersModule> module, ILogger logger)
        {
            this._module = module;
            this._logger = logger;
        }

        public Task<QueryResult<TQuery, TResult>> ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            try
            {
                this._logger.Information("Readers module executing query of type " + query?.GetType().Name);

                var task = this._module.ExecuteQueryAsync<TQuery, TResult>(query);

                this._logger.Information("Query of type " + query?.GetType().Name + " was executed with success.");

                return task;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, "Error while processing query of type " + query?.GetType().Name + ".");

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
                this._logger.Information("Readers module sending command of type " + command.GetType().Name);

                validationResult = await this._module.SendCommandAsync(command);

                this._logger.Information(
                    $"Command of type {command.GetType()} was processed with result {validationResult.Status}");
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, "Command command processing failure.");

                throw;
            }

            return validationResult;
        }
    }
}