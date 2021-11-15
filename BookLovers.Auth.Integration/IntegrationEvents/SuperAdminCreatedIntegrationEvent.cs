using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Auth.Integration.IntegrationEvents
{
    public class SuperAdminCreatedIntegrationEvent : IIntegrationEvent
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

        private SuperAdminCreatedIntegrationEvent(
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

        private SuperAdminCreatedIntegrationEvent()
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.UserStatus = AggregateStatus.Active.Value;
        }

        public static SuperAdminCreatedIntegrationEvent Initialize()
        {
            return new SuperAdminCreatedIntegrationEvent();
        }

        public SuperAdminCreatedIntegrationEvent WithReader(
            int readerId,
            Guid readerGuid)
        {
            return new SuperAdminCreatedIntegrationEvent(
                readerId,
                readerGuid,
                this.BookcaseGuid,
                this.SocialProfileGuid,
                this.UserName,
                this.Email,
                this.AccountCreateDate);
        }

        public SuperAdminCreatedIntegrationEvent WithBookcase(
            Guid bookcaseGuid)
        {
            return new SuperAdminCreatedIntegrationEvent(
                this.ReaderId,
                this.ReaderGuid,
                bookcaseGuid,
                this.SocialProfileGuid,
                this.UserName,
                this.Email,
                this.AccountCreateDate);
        }

        public SuperAdminCreatedIntegrationEvent WithProfile(
            Guid profileGuid)
        {
            return new SuperAdminCreatedIntegrationEvent(
                this.ReaderId,
                this.ReaderGuid,
                this.BookcaseGuid,
                profileGuid,
                this.UserName,
                this.Email,
                this.AccountCreateDate);
        }

        public SuperAdminCreatedIntegrationEvent WithAccount(
            string userName,
            string email,
            DateTime accountCreateDate)
        {
            return new SuperAdminCreatedIntegrationEvent(
                this.ReaderId,
                this.ReaderGuid,
                this.BookcaseGuid,
                this.SocialProfileGuid,
                userName,
                email,
                accountCreateDate);
        }

        public SuperAdminCreatedIntegrationEvent Build()
        {
            return new SuperAdminCreatedIntegrationEvent(
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