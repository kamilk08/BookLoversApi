using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Dsl;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Shared.SharedSexes;

namespace BaseTests.DataCreationHelpers
{
    public static class AuthorFactoryFixtureExtensions
    {
        public static IPostprocessComposer<AuthorWriteModel> InitializeAuthorDto(
            this Fixture fixture)
        {
            return fixture.Build<AuthorWriteModel>()
                .With(p => p.Basics)
                .With(p => p.Description)
                .With(p => p.Details)
                .With(p => p.AuthorBooks)
                .With(p => p.AuthorGenres)
                .With(p => p.AuthorGuid);
        }

        public static IPostprocessComposer<AuthorWriteModel> WithBooks(
            this IPostprocessComposer<AuthorWriteModel> composer,
            List<Guid> bookGuides)
        {
            return composer.With(p => p.AuthorBooks, () => bookGuides);
        }

        public static IPostprocessComposer<AuthorWriteModel> WithBasics(
            this IPostprocessComposer<AuthorWriteModel> composer,
            Sex sex,
            string firstName = null,
            string secondName = null)
        {
            return composer.With(p => p.Basics, () => new AuthorBasicsWriteModel
            {
                FirstName = firstName ?? composer.Create<string>(),
                SecondName = secondName ?? composer.Create<string>(),
                Sex = sex.Value
            });
        }

        public static IPostprocessComposer<AuthorWriteModel> WithGenres(
            this IPostprocessComposer<AuthorWriteModel> composer,
            List<int> genres)
        {
            return composer.With(p => p.AuthorGenres, genres);
        }

        public static IPostprocessComposer<AuthorWriteModel> WithDescription(
            this IPostprocessComposer<AuthorWriteModel> composer,
            string about,
            string source,
            string webSite)
        {
            return composer.With(p => p.Description, () => new AuthorDescriptionWriteModel
            {
                AboutAuthor = about,
                DescriptionSource = source,
                WebSite = webSite
            });
        }

        public static IPostprocessComposer<AuthorWriteModel> WithDetails(
            this IPostprocessComposer<AuthorWriteModel> composer,
            DateTime? birthDate,
            string birthPlace,
            DateTime? deathDate)
        {
            return composer.With(p => p.Details, () => new AuthorDetailsWriteModel
            {
                BirthDate = birthDate,
                BirthPlace = birthPlace,
                DeathDate = deathDate
            });
        }
    }
}