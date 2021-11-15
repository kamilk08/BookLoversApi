using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Mementos
{
    public interface IReaderMemento : IMemento<Reader>, IMemento
    {
        IList<IAddedResource> AddedResources { get; }

        IList<Activity> Activities { get; }

        IList<Guid> Followers { get; }

        Guid SocialProfileGuid { get; }

        Guid NotificationWallGuid { get; }

        Guid StatisticsGathererGuid { get; }

        Guid TimeLineGuid { get; }

        string UserName { get; }

        string Email { get; }

        int ReaderId { get; }
    }
}