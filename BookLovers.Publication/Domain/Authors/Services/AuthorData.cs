using System;
using System.Collections.Generic;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public class AuthorData
    {
        public Guid AuthorGuid { get; private set; }

        public BookReader BookReader { get; private set; }

        public AuthorBasicsData Basics { get; private set; }

        public AuthorDetailsData Details { get; private set; }

        public AuthorDescriptionData Description { get; private set; }

        public List<int> Genres { get; private set; }

        public List<Guid> AuthorBooks { get; private set; }

        private AuthorData()
        {
        }

        public static AuthorData Initialize()
        {
            return new AuthorData();
        }

        public AuthorData WithGuid(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;

            return this;
        }

        public AuthorData AddedBy(BookReader bookReader)
        {
            this.BookReader = bookReader;

            return this;
        }

        public AuthorData WithGenres(List<int> genresIds)
        {
            this.Genres = genresIds;

            return this;
        }

        public AuthorData WithBooks(List<Guid> bookGuides)
        {
            this.AuthorBooks = bookGuides;

            return this;
        }

        public AuthorData WithBasics(FullName fullName, Sex sex)
        {
            this.Basics = new AuthorBasicsData(fullName, sex);

            return this;
        }

        public AuthorData WithDetails(LifeLength lifeLength, string birthPlace)
        {
            this.Details = new AuthorDetailsData(lifeLength, birthPlace);

            return this;
        }

        public AuthorData WithDescription(
            string aboutAuthor,
            string description,
            string webSite)
        {
            this.Description =
                new AuthorDescriptionData(aboutAuthor, description, webSite);

            return this;
        }
    }
}