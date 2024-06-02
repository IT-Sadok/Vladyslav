using Application.Abstractions;
using Domain.Entities;
using Healthcare.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _appDbContext;

    public AppointmentRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;
    public async Task CreateAppiontmentAsync(Appointment appointment)
    {
        await _appDbContext.Appointments.AddAsync(appointment);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<Appointment>> GetAllAppointmentsAsync() => await _appDbContext.Appointments.ToListAsync();
}