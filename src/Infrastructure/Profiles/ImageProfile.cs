namespace Infrastructure.Profiles
{
    using AutoMapper;
    using Domain.DTO;
    using Domain.Entities;

    public class ImageProfile : Profile
    {
        public ImageProfile() 
        {
            CreateMap<ImageDTO, Image>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ImageData, opt => opt.MapFrom(src => src.ImageData))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Image, ImageDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ImageData, opt => opt.MapFrom(src => src.ImageData))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));
                
        }
    }
}
