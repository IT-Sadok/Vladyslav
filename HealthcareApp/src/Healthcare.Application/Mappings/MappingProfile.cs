using Application.DTOs.Register;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Healthcare.Application.Appointments.Commands.Book;
using Healthcare.Application.Appointments.Commands.Upsert;
using static Domain.Constants.AppointmentStatuses;


namespace Healthcare.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookAppointmentCommand, Appointment>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Requested));

        CreateMap<UpsertAppointmentCommand, Appointment>();

        CreateMap<RegisterUserDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
    } 
}