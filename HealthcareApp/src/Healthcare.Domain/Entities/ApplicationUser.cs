using System.Xml.Serialization;
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    
    [XmlIgnore]
    public ICollection<Appointment> DoctorAppointments { get; set; }
    [XmlIgnore]
    public ICollection<Appointment> PatientAppointments { get; set; }
}