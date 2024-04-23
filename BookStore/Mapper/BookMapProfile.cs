using AutoMapper;
using BookStore.DTOs;
using BookStore.Entities;

namespace BookStore.Mapper;

public class BookMapProfile:Profile
{
    public BookMapProfile()
    {
        CreateMap<Book, BookGetDto>().ReverseMap();
        CreateMap<Book, BookPostDto>().ReverseMap();
    }
}
