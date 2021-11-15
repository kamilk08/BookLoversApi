using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using Serilog;

namespace BookLovers.Bookcases.Infrastructure.Root.Events
{
    internal class ProjectionDispatcherLoggingDecorator : IProjectionDispatcher
    {
        private readonly IProjectionDispatcher _dispatcher;
        private readonly ILogger _logger;

        public ProjectionDispatcherLoggingDecorator(IProjectionDispatcher dispatcher, ILogger logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            try
            {
                _logger.Information("Module applying event projection of type " + @event.GetType().Name + ".");

                _dispatcher.Dispatch(@event);

                _logger.Information("Module applied successfully projection of type " + @event.GetType().Name + ".");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while processing projection of type " + @event.GetType().Name + ".");

                throw;
            }
        }
    }
}