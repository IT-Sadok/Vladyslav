using Domain.Entities;

namespace Application.Abstractions;

public interface IAppointmentRepository
{
    Task RequestAppointmentAsync(Appointment appointment);
    Task<List<Appointment>> GetAllAppointmentsAsync();
    
    Task<List<Appointment>> GetRequestedAppointments(string doctorId);

}