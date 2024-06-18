using System.Xml;
using System.Xml.Serialization;
using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Constants;
using Domain.Entities;
using MigrationAdminPanel.Abstractions;
using MigrationAdminPanel.DataTypes;

namespace MigrationAdminPanel.Services;

public class XmlMigrationsService : MigrationsService
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IAppointmentRepository _appointmentRepository;

    public XmlMigrationsService(IUserManagerDecorator<ApplicationUser> userManager,
        IAppointmentRepository appointmentRepository)
    {
        _userManager = userManager;
        _appointmentRepository = appointmentRepository;
    }

    public override async Task MigrateData(string path)
    {
        try
        {
            // TODO
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}