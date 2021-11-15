﻿using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using Serilog;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal class GenericProducerLoggingNotifier<T> : IProducerNotification<T>, IProducerNotification
        where T : IIntegrationEvent
    {
        private readonly IProducerNotification<T> _decoratedNotifier;
        private readonly ILogger _logger;

        public GenericProducerLoggingNotifier(
            IProducerNotification<T> decoratedNotifier,
            ILogger logger)
        {
            this._decoratedNotifier = decoratedNotifier;
            this._logger = logger;
        }

        public void NotifyProducer(T @event, Dictionary<int, bool> map)
        {
            try
            {
                this._logger.Information("Notifying producer of the event of type " + @event.GetType().FullName);

                this._decoratedNotifier.NotifyProducer(@event, map);

                this._logger.Information("Producer of the event of type " + @event.GetType().FullName +
                                         " notified successfully");
            }
            catch (Exception ex)
            {
                this._logger.Information(ex, "Could not notify producer of the integration event of type $" + @event.GetType().FullName);

                throw;
            }
        }
    }
}