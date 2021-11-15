using System;
using BookLovers.Readers.Application.WriteModels.Profiles;
using BookLovers.Readers.Application.WriteModels.Reviews;
using BookLovers.Readers.Domain.Profiles.Services.Factories;
using BookLovers.Readers.Domain.Reviews.Services;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Readers.Application.Contracts.Conversions
{
    internal static class ConversionExtensions
    {
        internal static ProfileData ConvertToProfileData(this ProfileWriteModel writeModel)
        {
            var details = ProfileDetailsData
                .Initialize()
                .WithCity(writeModel.Address.City)
                .WithCountry(writeModel.Address.Country)
                .WithAboutUser(writeModel.About.Content)
                .WithWebSite(writeModel.About.WebSite);

            var content = new ProfileContentData(
                new FullName(writeModel.Identity.FirstName, writeModel.Identity.SecondName),
                writeModel.Identity.BirthDate, Sexes.Get(writeModel.Identity.Sex));

            return ProfileData
                .Initialize()
                .WithProfile(writeModel.ProfileGuid)
                .WithContent(content)
                .WithDetails(details);
        }

        internal static ReviewParts ConvertToReviewParts(
            this ReviewWriteModel writeModel,
            Guid readerGuid)
        {
            return ReviewParts
                .Initialize()
                .WithGuid(writeModel.ReviewGuid)
                .WitBook(writeModel.BookGuid)
                .AddedBy(readerGuid)
                .WithContent(writeModel.ReviewDetails.Content)
                .WithDates(writeModel.ReviewDetails.ReviewDate, writeModel.ReviewDetails.EditDate)
                .HasSpoilers(writeModel.ReviewDetails.MarkedAsSpoiler);
        }
    }
}