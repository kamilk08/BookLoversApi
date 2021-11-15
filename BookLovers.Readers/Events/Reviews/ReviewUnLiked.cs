using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewUnLiked : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid LikeGiverGuid { get; private set; }

        public Guid LikeReceiverGuid { get; private set; }

        public int Likes { get; private set; }

        private ReviewUnLiked()
        {
        }

        [JsonConstructor]
        protected ReviewUnLiked(
            Guid guid,
            Guid aggregateGuid,
            Guid likeGiverGuid,
            Guid likeReceiverGuid,
            int likes)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            LikeGiverGuid = likeGiverGuid;
            LikeReceiverGuid = likeReceiverGuid;
            Likes = likes;
        }

        public ReviewUnLiked(Guid aggregateGuid, Guid likeGiverGuid, Guid likeReceiverGuid, int likes)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            LikeGiverGuid = likeGiverGuid;
            LikeReceiverGuid = likeReceiverGuid;
            Likes = likes;
        }
    }
}