using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Auth.Integration.IntegrationEvents
{
    public class UserSignedUpIntegrationEvent : IIntegrationEvent
    {
        public int ReaderId { get; set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid SocialProfileGuid { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }

        public DateTime AccountCreateDate { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public int UserStatus { get; private set; }

        private UserSignedUpIntegrationEvent(
            int readerId,
            Guid readerGuid,
            Guid bookcaseGuid,
            Guid socialProfileGuid,
            string userName,
            string email,
            DateTime accountCreateDate)
            : this()
        {
            this.ReaderId = readerId;
            this.ReaderGuid = readerGuid;
            this.BookcaseGuid = bookcaseGuid;
            this.SocialProfileGuid = socialProfileGuid;
            this.UserName = userName;
            this.Email = email;
            this.AccountCreateDate = accountCreateDate;
        }

        private UserSignedUpIntegrationEvent()
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.UserStatus = AggregateStatus.Active.Value;
        }

        public static UserSignedUpIntegrationEvent Initialize()
        {
            return new UserSignedUpIntegrationEvent();
        }

        public UserSignedUpIntegrationEvent WithReader(
            int readerId,
            Guid readerGuid)
        {
            return new UserSignedUpIntegrationEvent(
                readerId,
                readerGuid,
                this.BookcaseGuid,
                this.SocialProfileGuid,
                this.UserName,
                this.Email,
                this.AccountCreateDate);
        }

        public UserSignedUpIntegrationEvent WithBookcase(Guid bookcaseGuid)
        {
            return new UserSignedUpIntegrationEvent(
                this.ReaderId,
                this.ReaderGuid,
                bookcaseGuid,
                this.SocialProfileGuid,
                this.UserName,
                this.Email,
                this.AccountCreateDate);
        }

        public UserSignedUpIntegrationEvent WithProfile(Guid profileGuid)
        {
            return new UserSignedUpIntegrationEvent(
                this.ReaderId,
                this.ReaderGuid,
                this.BookcaseGuid,
                profileGuid,
                this.UserName,
                this.Email,
                this.AccountCreateDate);
        }

        public UserSignedUpIntegrationEvent WithAccount(
            string userName,
            string email,
            DateTime accountCreateDate)
        {
            return new UserSignedUpIntegrationEvent(
                this.ReaderId,
                this.ReaderGuid,
                this.BookcaseGuid,
                this.SocialProfileGuid,
                userName,
                email,
                accountCreateDate);
        }

        public UserSignedUpIntegrationEvent Build()
        {
            return new UserSignedUpIntegrationEvent(
                this.ReaderId,
                this.ReaderGuid,
                this.BookcaseGuid,
                this.SocialProfileGuid,
                this.UserName,
                this.Email,
                this.AccountCreateDate);
        }
    }
}