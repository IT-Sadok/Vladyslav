using Application.Abstractions.Decorators;
using Domain.Constants;
using Domain.Entities;

namespace MigrationAdminPanel.Abstractions
{
    public abstract class MigrationsService
    {
        protected readonly IUserManagerDecorator<ApplicationUser> UserManager;
        
        protected MigrationsService(IUserManagerDecorator<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task MigrateData(string path)
        {
            try
            {
                var data = await ReadDataFromFile(path);
                
                if (data == null)
                {
                    Console.WriteLine("Deserialization failed, migrationsData is null.");
                    return;
                }

                var (patients, doctors) = ExtractUserData(data);

                await UserManager.MigrateRangeAsync(patients, doctors);

                foreach (var patient in patients)
                {
                    patient.UserName = patient.Email;
                    await UserManager.AddToRoleAsync(patient, UserRolesConstants.Patient);
                }

                foreach (var doctor in doctors)
                {
                    doctor.UserName = doctor.Email;
                    await UserManager.AddToRoleAsync(doctor, UserRolesConstants.Doctor);
                }

                await MigrateAdditionalData(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        protected abstract Task<object?> ReadDataFromFile(string path);
        protected abstract (List<ApplicationUser> patients, List<ApplicationUser> doctors) ExtractUserData(object data);
        protected virtual Task MigrateAdditionalData(object data) => Task.CompletedTask;
    }
}