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
        string baseDirectory = AppContext.BaseDirectory;
        
        string jsonFilePath = Path.Combine(baseDirectory, "..", "..", "..", "Data", "sampleData.json");
        string xmlFilePath = Path.Combine(baseDirectory, "..", "..", "..", "Data", "sampleData.xml");
        
        jsonFilePath = Path.GetFullPath(jsonFilePath);
        xmlFilePath = Path.GetFullPath(xmlFilePath);

        await _jsonMigrator.MigrateData(jsonFilePath);
        await _xmlMigrator.MigrateData(xmlFilePath);
    }
}