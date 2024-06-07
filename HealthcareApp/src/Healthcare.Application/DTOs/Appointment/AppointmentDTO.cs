using System.Data.Common;
using Domain.Constants;

namespace Healthcare.Application.DTOs.Appointment;

public record AppointmentDTO(int Id,
    string PatientId, 
    DateTime AppointmentDate, 
    TimeSpan StartTime, 
    TimeSpan EndTime, 
    string Status);