using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Dsl;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Shared.Categories;

namespace BaseTests.DataCreationHelpers
{
    public static class BookFactoryFixtureExtensions
    {
        public static IPostprocessComposer<BookWriteModel> InitializeBookDto(
            this Fixture fixture)
        {
            return fixture.Build<BookWriteModel>()
                .With(p => p.Authors)
                .With(p => p.Cycles)
                .With(p => p.AddedBy)
                .With(p => p.BookGuid)
                .With(p => p.HashTags)
                .With(p => p.SeriesGuid)
                .With(p => p.PositionInSeries);
        }

        public static IPostprocessComposer<BookWriteModel> WithBookBasics(
            this IPostprocessComposer<BookWriteModel> composer,
            Category category,
            SubCategory subCategory,
            string isbn,
            string title = null,
            Guid? publisherGuid = null,
            DateTime? date = null)
        {
            return composer.With(p => p.Basics, () =>
            {
                var basicsWriteModel = new BookBasicsWriteModel();

                basicsWriteModel.Category = category != null ? category.Value : -1;
                basicsWriteModel.Isbn = isbn;
                basicsWriteModel.Title = title ?? composer.Create<string>();
                basicsWriteModel.PublicationDate = date ?? composer.Create<DateTime>();
                basicsWriteModel.PublisherGuid = publisherGuid ?? composer.Create<Guid>();
                basicsWriteModel.SubCategory = subCategory != null ? subCategory.Value : -1;

                return basicsWriteModel;
            });
        }

        public static IPostprocessComposer<BookWriteModel> WithCover(
            this IPostprocessComposer<BookWriteModel> composer,
            CoverType coverType,
            bool isCoverAdded,
            string coverSource = null)
        {
            return composer.With(p => p.Cover, () =>
            {
                var bookCoverWriteModel = new BookCoverWriteModel();
                bookCoverWriteModel.CoverSource = coverSource == null ? null : composer.Create<string>();
                bookCoverWriteModel.CoverType = coverType != null
                    ? coverType.Value
                    : CoverType.NoCover.Value;
                bookCoverWriteModel.IsCoverAdded = isCoverAdded;

                return bookCoverWriteModel;
            });
        }

        public static IPostprocessComposer<BookWriteModel> WithDetails(
            this IPostprocessComposer<BookWriteModel> composer,
            int? pages,
            Language language)
        {
            return composer.With(p => p.Details, () =>
            {
                var detailsWriteModel = new BookDetailsWriteModel();

                detailsWriteModel.Language = language != null ? language.Value : Language.Unknown.Value;
                detailsWriteModel.Pages = pages;

                return detailsWriteModel;
            });
        }

        public static IPostprocessComposer<BookWriteModel> WithCycles(
            this IPostprocessComposer<BookWriteModel> composer,
            List<Guid> cycleGuides)
        {
            return composer.With(p => p.Cycles, () => cycleGuides);
        }

        public static IPostprocessComposer<BookWriteModel> WithDescription(
            this IPostprocessComposer<BookWriteModel> composer,
            string description,
            string source)
        {
            return composer.With(p => p.Description, () => new BookDescriptionWriteModel
            {
                Content = description,
                DescriptionSource = source
            });
        }

        public static IPostprocessComposer<BookWriteModel> WithSeries(
            this IPostprocessComposer<BookWriteModel> composer,
            Guid? series,
            byte? positionInSeries)
        {
            return composer
                .With(p => p.SeriesGuid, series)
                .With(p => p.PositionInSeries, positionInSeries);
        }
    }
}