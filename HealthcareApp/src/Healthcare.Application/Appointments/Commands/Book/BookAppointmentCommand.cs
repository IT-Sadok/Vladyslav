using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Appointments.Commands.Book;

public record BookAppointmentCommand(
    string DoctorId,
    string PatientId, 
    DateTime AppointmentDate, 
    TimeSpan StartTime) 
    : IRequest<Result>;