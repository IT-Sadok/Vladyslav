using Microsoft.Extensions.DependencyInjection;
using MigrationAdminPanel.Configuration;

var host = HostBuilderConfigurator.CreateHostBuilder(args).Build();

var app = host.Services.GetRequiredService<MyApp>();
await app.Run();