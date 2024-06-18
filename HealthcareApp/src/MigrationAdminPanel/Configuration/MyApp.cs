using MigrationAdminPanel.Services;

namespace MigrationAdminPanel.Configuration;

public class MyApp
{
    
    private readonly JsonMigrationsService _jsonMigrator;
    private readonly XmlMigrationsService _xmlMigrator;


    public MyApp(JsonMigrationsService jsonMigrator, XmlMigrationsService xmlMigrator)
    {
        _jsonMigrator = jsonMigrator;
        _xmlMigrator = xmlMigrator;
    }

    public async Task Run()
    {
       // await _jsonMigrator.MigrateData("/Users/vladmakarenko/Desktop/Dotnet Mentorship/HealthcareApp/src/MigrationAdminPanel/Data/sampleData.json");
        await _xmlMigrator.MigrateData("/Users/vladmakarenko/Desktop/Dotnet Mentorship/HealthcareApp/src/MigrationAdminPanel/Data/sampleData.xml");
    }
}