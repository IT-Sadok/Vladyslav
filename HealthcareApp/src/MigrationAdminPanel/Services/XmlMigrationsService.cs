using Application.Abstractions.Decorators;
using Domain.Entities;
using MigrationAdminPanel.Abstractions;
using MigrationAdminPanel.DataTypes;
using System.Xml.Serialization;
using Application.Abstractions;

namespace MigrationAdminPanel.Services
{
    public class XmlMigrationsService : MigrationsService
    {
        private readonly IMigrationsRepository _repository;

        public XmlMigrationsService(IMigrationsRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        protected override async Task<object?> ReadDataFromFile(string path)
        {
            try
            {
                var xmlContent = await File.ReadAllTextAsync(path);
                var ser = new XmlSerializer(typeof(XmlMigrationsData));
                using var reader = new StringReader(xmlContent);

                return ser.Deserialize(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while reading file: {e.Message}");
                throw;
            }
        }

        protected override (List<ApplicationUser> patients, List<ApplicationUser> doctors) ExtractUserData(object data)
        {
            var migrationsData = (XmlMigrationsData)data;
            return (migrationsData.Patients, migrationsData.Doctors);
        }

        protected override List<Appointment>? ExtractAdditionalData(object data) => null;
    }
}