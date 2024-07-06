using Application.Abstractions;
using Dapper;
using Domain.Constants;
using Domain.Entities;
using Healthcare.Application.DTOs.Appointment;
using Healthcare.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using static Healthcare.Infrastructure.Persistance.SqlCommands;

namespace Infrastructure.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly SqlConnectionFactory _dbConnectionFactory;


    public AppointmentRepository(AppDbContext appDbContext, SqlConnectionFactory dbConnectionFactory)
    {
        _appDbContext = appDbContext;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task RequestAppointmentAsync(Appointment appointment)
    {
        await _appDbContext.Appointments.AddAsync(appointment);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<AppointmentDTO>> GetDoctorAppointments(string doctorId, int pageSize)
    {
        return await _appDbContext.Appointments.Where(x => x.DoctorId == doctorId)
            .OrderBy(x => x.AppointmentDate)
            .Take(pageSize).Select(x =>
                new AppointmentDTO(x.Id, x.PatientId, x.AppointmentDate, x.StartTime, x.EndTime, x.Status.ToString()))
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int appointmentId) =>
        await _appDbContext.Appointments.FindAsync(appointmentId);

    public async Task ChangeStatusAsync(int appointmentId, AppointmentStatuses status)
    {
        var appointment = await GetByIdAsync(appointmentId);
        if (appointment != null)
        {
            appointment.Status = status;
            _appDbContext.Update(appointment);
            await _appDbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> IsAvailableAsync(string doctorId, DateTime date, TimeSpan startTime)
    {
        var endTime = startTime.Add(TimeSpan.FromMinutes(15));

        var existingAppointment = await _appDbContext.Appointments.SingleOrDefaultAsync(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDate == date &&
            ((a.StartTime <= startTime && a.EndTime > startTime) ||
             (a.StartTime < endTime && a.EndTime >= endTime)));

        return existingAppointment == null;
    }

    public async Task UpsertAppointmentAsync(Appointment appointment)
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();

        const string query = UpsertAppointment;
        
        await sqlConnection.ExecuteAsync(query, appointment);
    }
}