using Application.Abstractions.Decorators;
using Application.Abstractions;
using Domain.Entities;
using MigrationAdminPanel.Abstractions;
using MigrationAdminPanel.DataTypes;
using Newtonsoft.Json;

namespace MigrationAdminPanel.Services
{
    public class JsonMigrationsService : MigrationsService
    {
        private readonly IMigrationsRepository _repository;

        public JsonMigrationsService(IMigrationsRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        protected override async Task<object?> ReadDataFromFile(string path)
        {
            string jsonString = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<JsonMigrationsData>(jsonString);
        }

        protected override (List<ApplicationUser> patients, List<ApplicationUser> doctors) ExtractUserData(object data)
        {
            var migrationsData = (JsonMigrationsData)data;
            return (migrationsData.Patients, migrationsData.Doctors);
        }

        protected override List<Appointment>? ExtractAdditionalData(object data)
        {
            var migrationsData = (JsonMigrationsData)data;
            return migrationsData.Appointments;
        }
    }
}