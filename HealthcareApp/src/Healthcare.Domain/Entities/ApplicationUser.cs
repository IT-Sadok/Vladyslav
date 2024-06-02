using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    
    public ICollection<Appointment> DoctorAppointments { get; set; }
    public ICollection<Appointment> PatientAppointments { get; set; }
}