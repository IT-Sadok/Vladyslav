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
        private readonly IAppointmentRepository _appointmentRepository;

        public JsonMigrationsService(IUserManagerDecorator<ApplicationUser> userManager,
            IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository)
            : base(userManager, scheduleRepository)
        {
            _appointmentRepository = appointmentRepository;
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

        protected override async Task MigrateAdditionalData(object data)
        {
            var migrationsData = (JsonMigrationsData)data;
            var appointments = migrationsData.Appointments;
            await _appointmentRepository.MigrateRangeAsync(appointments);
        }
    }
}