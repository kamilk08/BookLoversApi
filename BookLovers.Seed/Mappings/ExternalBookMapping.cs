using System;
using System.Linq;
using AutoMapper;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.OpenLibrary.Books;

namespace BookLovers.Seed.Mappings
{
    public class ExternalBookMapping : Profile
    {
        public ExternalBookMapping()
        {
            this.CreateMap<BookRoot, SeedBook>()
                .ForMember(
                    dest => dest.Isbn,
                    opt => opt.MapFrom(p => p.Isbn13.Length >= 1 ? p.Isbn13.First() : p.Isbn10.First()))
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(p => p.Authors.Select(s => s.Key)))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(p => p.Description != null ? p.Description.Value : Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Pages, opt => opt.MapFrom(p => int.Parse(p.Pages)))
                .ForMember(dest => dest.Published, opt => opt.MapFrom(s => s.PublishDate))
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(p => p.Publishers.First()))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(p => p.Title))
                .ForMember(dest => dest.BookGuid, opt => opt.MapFrom((p) => Guid.NewGuid()));
        }
    }
}