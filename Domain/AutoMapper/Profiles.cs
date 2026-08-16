using AutoMapper;
using Domain.DTOs;
using Domain.Models;

namespace Domain.AutoMapper
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<DocFiscal, DocFiscalDto>().ReverseMap();
        }
    }
}
