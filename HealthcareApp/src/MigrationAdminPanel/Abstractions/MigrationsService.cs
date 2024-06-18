using Application.Abstractions.Decorators;
using Domain.Entities;

namespace MigrationAdminPanel.Abstractions;

public abstract class MigrationsService
{
    public virtual Task MigrateData(string path)
    {
        return Task.CompletedTask;
    }
}