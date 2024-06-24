using Application.Abstractions;
using Domain.Entities;

namespace MigrationAdminPanel.Abstractions
{
    public abstract class MigrationsService
    {
        private readonly IMigrationsRepository _migrationsRepository;

        protected MigrationsService(IMigrationsRepository migrationsRepository)
        {
            _migrationsRepository = migrationsRepository;
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

                var appointments = ExtractAdditionalData(data);

                await _migrationsRepository.MigrateRangeAsync(patients, doctors, appointments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        protected abstract Task<object?> ReadDataFromFile(string path);
        protected abstract (List<ApplicationUser> patients, List<ApplicationUser> doctors) ExtractUserData(object data);
        protected abstract List<Appointment>? ExtractAdditionalData(object data);
    }
}