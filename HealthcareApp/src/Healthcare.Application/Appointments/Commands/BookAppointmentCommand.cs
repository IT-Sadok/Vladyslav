using MediatR;

namespace Healthcare.Application.Appointments.Commands;

public record BookAppointmentCommand(string DoctorId, string PatientId, DateTime AppointmentDate, TimeSpan StartTime)
    : IRequest<bool>;