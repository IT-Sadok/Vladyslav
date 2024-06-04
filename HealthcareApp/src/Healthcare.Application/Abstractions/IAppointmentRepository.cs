using Domain.Entities;

namespace Application.Abstractions;

public interface IAppointmentRepository
{
    Task RequestAppointmentAsync(Appointment appointment);
    Task<List<Appointment>> GetAllAppointmentsAsync();
    Task<List<Appointment>> GetRequestedAppointments(string doctorId);
    Task<Appointment?> GetByIdAsync(int appointmentId);
    Task SubmitAppointmentAsync(int appointmentId);
    Task RejectAppointmentAsync(int appointmentId);
    Task CompleteAppointmentAsync(int appointmentId);
}