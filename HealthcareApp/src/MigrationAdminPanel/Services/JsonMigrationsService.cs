using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Constants;
using Domain.Entities;
using MigrationAdminPanel.Abstractions;
using MigrationAdminPanel.DataTypes;
using Newtonsoft.Json;

namespace MigrationAdminPanel.Services;

public class JsonMigrationsService : MigrationsService
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IAppointmentRepository _appointmentRepository;

    public JsonMigrationsService(IUserManagerDecorator<ApplicationUser> userManager,
        IAppointmentRepository appointmentRepository)
    {
        _userManager = userManager;
        _appointmentRepository = appointmentRepository;
    }

    public override async Task MigrateData(string path)
    {
        try
        {
            var jsonString = await File.ReadAllTextAsync(path);

            var migrationsData = JsonConvert.DeserializeObject<JsonMigrationsData>(jsonString);

            // Check if migrationsData is null
            if (migrationsData == null)
            {
                Console.WriteLine("Deserialization failed, migrationsData is null.");
                return;
            }


            List<ApplicationUser> patients = migrationsData.Patients;
            List<ApplicationUser> doctors = migrationsData.Doctors;
            List<Appointment> appointments = migrationsData.Appointments;

            await _userManager.MigrateRangeAsync(patients, doctors);
            await _appointmentRepository.MigrateRangeAsync(appointments);

            foreach (var patient in patients)
            {
                patient.UserName = patient.Email;
                await _userManager.AddToRoleAsync(patient, UserRolesConstants.Patient);
            }

            foreach (var doctor in doctors)
            {
                doctor.UserName = doctor.Email;
                await _userManager.AddToRoleAsync(doctor, UserRolesConstants.Doctor);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}