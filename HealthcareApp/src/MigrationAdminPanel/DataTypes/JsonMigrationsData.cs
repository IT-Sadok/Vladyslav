using System.Text.Json.Serialization;
using Domain.Entities;
using MigrationAdminPanel.Abstractions;

namespace MigrationAdminPanel.DataTypes;

public class JsonMigrationsData
{
    [JsonPropertyName("patients")]
    public List<ApplicationUser> Patients { get; set; }
    
    [JsonPropertyName("doctors")]
    public List<ApplicationUser> Doctors { get; set; }
    [JsonPropertyName("appointments")]
    public List<Appointment> Appointments { get; set; }
}