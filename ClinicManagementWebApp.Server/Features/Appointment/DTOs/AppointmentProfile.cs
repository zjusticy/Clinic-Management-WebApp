using AutoMapper;
using ClinicManagementWebApp.Server.Features.Appointment.Entity;

namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<AppointmentAddOrUpdateDTO, AppointmentModel>();
            CreateMap<AppointmentUpdateDTO, AppointmentModel>();
            CreateMap<AppointmentModel, AppointmentUpdateDTO>();
            CreateMap<AppointmentModel, AppointmentBriefDTO>();
            CreateMap<AppointmentModel, AppointmentListBriefDTO>();
        }
    }
}
