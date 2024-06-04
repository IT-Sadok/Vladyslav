using Application.Abstractions;
using Domain.Entities;
using Healthcare.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using static Domain.Constants.AppointmentStatusConstants;

namespace Infrastructure.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _appDbContext;

    public AppointmentRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;

    public async Task RequestAppointmentAsync(Appointment appointment)
    {
        await _appDbContext.Appointments.AddAsync(appointment);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<Appointment>> GetAllAppointmentsAsync() => await _appDbContext.Appointments.ToListAsync();

    public async Task<List<Appointment>> GetRequestedAppointments(string doctorId)
    {
        return await _appDbContext.Appointments.Where(x => x.DoctorId == doctorId && x.Status == Requested.ToString())
            
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int appointmentId) =>
        await _appDbContext.Appointments.FindAsync(appointmentId);

    public async Task SubmitAppointmentAsync(int appointmentId)
    {
        var appointment = await GetByIdAsync(appointmentId);
        if (appointment != null)
        {
            appointment.Status = Submitted.ToString();
            _appDbContext.Update(appointment);
            await _appDbContext.SaveChangesAsync();
        }
    }

    public async Task RejectAppointmentAsync(int appointmentId)
    {
        var appointment = await GetByIdAsync(appointmentId);
        if (appointment != null)
        {
            appointment.Status = Rejected.ToString();
            _appDbContext.Update(appointment);
            await _appDbContext.SaveChangesAsync();
        }
    }

    public async Task CompleteAppointmentAsync(int appointmentId)
    {
        var appointment = await GetByIdAsync(appointmentId);
        if (appointment != null)
        {
            appointment.Status = Completed.ToString();
            _appDbContext.Update(appointment);
            await _appDbContext.SaveChangesAsync();
        }
    }
}