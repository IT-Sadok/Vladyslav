using MigrationAdminPanel.Services;

namespace MigrationAdminPanel.Configuration
{
    public class MyApp
    {
        private readonly JsonMigrationsService _jsonMigrator;
        private readonly XmlMigrationsService _xmlMigrator;

        private const string JsonRelativePath  = @"/Users/vladmakarenko/Desktop/Dotnet Mentorship/HealthcareApp/src/MigrationAdminPanel/Data/sampleData.json";
        private const string XmlRelativePath = @"/Users/vladmakarenko/Desktop/Dotnet Mentorship/HealthcareApp/src/MigrationAdminPanel/Data/sampleData.xml";

        public MyApp(JsonMigrationsService jsonMigrator, XmlMigrationsService xmlMigrator)
        {
            _jsonMigrator = jsonMigrator;
            _xmlMigrator = xmlMigrator;
        }

        public async Task Run()
        {
            await _jsonMigrator.MigrateData(JsonRelativePath);
            await _xmlMigrator.MigrateData(XmlRelativePath);
        }
    }
}