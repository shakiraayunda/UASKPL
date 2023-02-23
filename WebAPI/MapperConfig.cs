using AutoMapper;
using WebAPI.Models;

namespace WebAPI
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserApi, UserApiDto>().ReverseMap();
        }
    }
}
