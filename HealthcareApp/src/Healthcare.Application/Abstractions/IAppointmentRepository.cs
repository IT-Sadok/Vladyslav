using Domain.Entities;

namespace Application.Abstractions;

public interface IAppointmentRepository
{
    Task CreateAppiontmentAsync(Appointment appointment);
    Task<List<Appointment>> GetAllAppointmentsAsync();
}