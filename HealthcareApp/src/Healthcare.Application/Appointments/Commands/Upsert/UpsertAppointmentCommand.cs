using Domain.Constants;
using Domain.Entities;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Appointments.Commands.Upsert;

public record UpsertAppointmentCommand(
    int Id,
    string DoctorId,
    string PatientId,
    DateTime AppointmentDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    AppointmentStatuses Status) : IRequest<Result<string>>;