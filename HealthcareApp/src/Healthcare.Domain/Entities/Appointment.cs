using Domain.Constants;

namespace Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public string DoctorId { get; set; }
    public string PatientId { get; set; }
    public ApplicationUser Doctor { get; set; }
    public ApplicationUser Patient { get; set; }
    
    public DateTime AppointmentDate { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
    
    public AppointmentStatuses Status { get; set; }
    
    public int DurationMinutes { get; set; }
}
