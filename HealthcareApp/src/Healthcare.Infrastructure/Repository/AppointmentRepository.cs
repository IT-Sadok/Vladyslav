using Application.Abstractions;
using Dapper;
using Domain.Constants;
using Domain.Entities;
using Healthcare.Application.DTOs.Appointment;
using Healthcare.Infrastructure.Persistance;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

        const string query = """
                                         MERGE INTO Appointments AS target
                                         USING (SELECT @Id AS Id) AS source
                                         ON (target.Id = source.Id)
                                         WHEN MATCHED THEN 
                                             UPDATE SET 
                                                 DoctorId = @DoctorId,
                                                 PatientId = @PatientId,
                                                 AppointmentDate = @AppointmentDate,
                                                 StartTime = @StartTime,
                                                 EndTime = @EndTime,
                                                 Status = @Status,
                                                 DurationMinutes = @DurationMinutes
                                         WHEN NOT MATCHED THEN
                                             INSERT (DoctorId, PatientId, AppointmentDate, StartTime, EndTime, Status, DurationMinutes)
                                             VALUES (@DoctorId, @PatientId, @AppointmentDate, @StartTime, @EndTime, @Status, @DurationMinutes);
                             """;


        appointment.DurationMinutes = (int)(appointment.EndTime - appointment.StartTime).TotalMinutes;
        await sqlConnection.ExecuteAsync(query, appointment);
    }
}