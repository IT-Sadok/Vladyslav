using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Application.Appointments.Commands;

public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, bool>
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IAppointmentRepository _appointmentRepository;


    public BookAppointmentCommandHandler(IUserManagerDecorator<ApplicationUser> userManager,
        IAppointmentRepository appointmentRepository)
    {
        _userManager = userManager;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<bool> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var users = await _userManager.GetAllUsersAsync();

        var doctor = await users.FirstOrDefaultAsync(x => x.Id == request.DoctorId, cancellationToken);
        if (doctor == null) throw new Exception("Doctor not found");

        var patient = await users.FirstOrDefaultAsync(x => x.Id == request.DoctorId, cancellationToken);
        if (patient == null) throw new Exception("Patient not found");

        var endTime = request.StartTime.Add(TimeSpan.FromMinutes(15));


        var appointments = await _appointmentRepository.GetAllAppointmentsAsync();
        var existingAppointment = appointments.FirstOrDefault<Appointment>(a =>
            a.DoctorId == request.DoctorId.ToString() &&
            a.AppointmentDate == request.AppointmentDate.Date &&
            ((a.StartTime <= request.StartTime && a.EndTime > request.StartTime) ||
             (a.StartTime < endTime && a.EndTime >= endTime)));
        
        if (existingAppointment != null) throw new Exception("Appointment slot already booked");

        var appointment = new Appointment
        {
            DoctorId = request.DoctorId.ToString(),
            PatientId = request.PatientId.ToString(),
            AppointmentDate = request.AppointmentDate.Date,
            StartTime = request.StartTime,
            EndTime = endTime
        };

        await _appointmentRepository.CreateAppiontmentAsync(appointment);

        return true;
    }
}