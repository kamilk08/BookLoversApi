using System;
using System.Collections.Generic;
using System.IO;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Seed.Models.Configuration;
using BookLovers.Seed.Models.OpenLibrary.Books;
using Newtonsoft.Json;

namespace BookLovers.Seed.Deserializers
{
    public class OpenLibraryBookDeserializer
    {
        private string _path;

        public OpenLibraryBookDeserializer(IAppManager manager)
        {
            this._path = manager.GetConfigValue("Seed_Data");
            this._path = AppDomain.CurrentDomain.BaseDirectory + this._path;
            this._path = this._path ?? string.Empty;
        }

        internal List<BookRoot> Deserialize(
            OpenLibrarySeedConfiguration openLibrarySeedConfiguration)
        {
            var books = new List<BookRoot>();
            using (var fileStream = File.Open(_path, FileMode.Open))
            using (var streamReader = new StreamReader(fileStream))
            {
                while (!streamReader.EndOfStream)
                {
                    if (IsLimitExceeded(openLibrarySeedConfiguration.Limit, books.Count))
                        break;
                    try
                    {
                        var currentLine = streamReader.ReadLine();
                        var bookRoot = JsonConvert.DeserializeObject<BookRoot>(currentLine);
                        if (BookShouldBeAdded(openLibrarySeedConfiguration, bookRoot))
                            books.Add(bookRoot);

                        streamReader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // SWALLOW EXCEPTION
                        // IF BOOK IS NOT A VALID JSON THEN JUST LEAVE IT
                    }
                }
            }

            return books;
        }

        private bool IsLimitExceeded(int limit, int currentIndex) => currentIndex >= limit;

        private bool BookShouldBeAdded(
            OpenLibrarySeedConfiguration seedConfiguration,
            BookRoot bookRoot)
        {
            if (seedConfiguration.OnlyWithCovers && bookRoot.Covers == null) return false;
            if (seedConfiguration.OnlyWithDescription && bookRoot.Description == null) return false;

            if (seedConfiguration.OnlyWithAuthors && (bookRoot.Authors == null || bookRoot.Authors.Count == 0))
                return false;

            if (seedConfiguration.OnlyWithIsbn && (bookRoot.Isbn10 == null || bookRoot.Isbn13 == null))
                return false;

            if (seedConfiguration.PublishedFrom == null || seedConfiguration.PublishedTill == null)
                return true;

            var isParsed = DateTime.TryParse(bookRoot.PublishDate, out var date);
            if (isParsed == false)
                return false;

            return date <= seedConfiguration.PublishedTill && date >= seedConfiguration.PublishedFrom;
        }
    }
}