using Application.Abstractions.Decorators;
using Domain.Entities;
using MigrationAdminPanel.Abstractions;
using MigrationAdminPanel.DataTypes;
using System.Xml.Serialization;

namespace MigrationAdminPanel.Services
{
    public class XmlMigrationsService : MigrationsService
    {
        public XmlMigrationsService(IUserManagerDecorator<ApplicationUser> userManager)
            : base(userManager) { }

        protected override async Task<object?> ReadDataFromFile(string path)
        {
            string xmlContent = await File.ReadAllTextAsync(path);
            var ser = new XmlSerializer(typeof(XmlMigrationsData));
            using var reader = new StringReader(xmlContent);

            return ser.Deserialize(reader);
        }

        protected override (List<ApplicationUser> patients, List<ApplicationUser> doctors) ExtractUserData(object data)
        {
            var migrationsData = (XmlMigrationsData)data;
            return (migrationsData.Patients, migrationsData.Doctors);
        }
    }
}