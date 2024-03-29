using AutoMapper;
using ClinicManagementWebApp.Server.Features.Users.Entity;

namespace ClinicManagementWebApp.Server.Features.Users.DTOs
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddOrUpdateDTO, UserRegisterDTO>();
            CreateMap<UserRegisterDTO, UserModel>();
            CreateMap<UserModel, UserBriefDTO>();
            CreateMap<UserModel, UserIdNameDTO>();
        }

    }
}
