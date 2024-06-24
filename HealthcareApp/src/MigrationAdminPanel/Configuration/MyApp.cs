using Microsoft.Extensions.Configuration;
using MigrationAdminPanel.Services;
using System.Text.RegularExpressions;

namespace MigrationAdminPanel.Configuration
{
    public class MyApp
    {
        private readonly JsonMigrationsService _jsonMigrator;
        private readonly XmlMigrationsService _xmlMigrator;
        private readonly string _jsonPath;
        private readonly string _xmlPath;

        public MyApp(JsonMigrationsService jsonMigrator, XmlMigrationsService xmlMigrator, IConfiguration configuration)
        {
            _jsonMigrator = jsonMigrator;
            _xmlMigrator = xmlMigrator;
            
            var baseDirectory = RemoveUnwantedSegments(AppContext.BaseDirectory);
            _jsonPath = Path.Combine(baseDirectory, "Data/sampleData.json");
            _xmlPath = Path.Combine(baseDirectory, "Data/sampleData.xml");
        }

        public async Task Run()
        {
            await _jsonMigrator.MigrateData(_jsonPath);
            await _xmlMigrator.MigrateData(_xmlPath);
        }
        
        private static string RemoveUnwantedSegments(string baseDirectory)
        {
            var regex = new Regex(@"[/\\]bin[/\\]Debug[/\\]net\d+\.\d+[/\\]?", RegexOptions.IgnoreCase);

            // Remove the matched segment from the path
            var projectRoot = regex.Replace(baseDirectory, string.Empty);

            return projectRoot;
        }
    }
}