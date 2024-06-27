using Domain.Constants;
using Domain.Entities;
using Healthcare.Application.DTOs.Appointment;

namespace Application.Abstractions;

public interface IAppointmentRepository
{
    Task RequestAppointmentAsync(Appointment appointment);
    Task<List<AppointmentDTO>> GetDoctorAppointments(string doctorId, int pageSize);
    Task<Appointment?> GetByIdAsync(int appointmentId);
    Task ChangeStatusAsync(int appointmentId, AppointmentStatuses status);
    Task<bool> IsAvailableAsync(string doctorId, DateTime date, TimeSpan startTime);
    Task UpsertAppointmentAsync(Appointment appointment);
}