using System.Reflection;
using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Healthcare.Infrastructure.Persistance;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    public int? DatePart(string datePartArg, DateTime? date) => throw new Exception();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(u => u.DoctorAppointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(u => u.PatientAppointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Doctor)
            .WithMany()
            .HasForeignKey(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        var methodInfo =
            typeof(AppDbContext).GetRuntimeMethod(nameof(DatePart), new[] { typeof(string), typeof(DateTime) });
        modelBuilder
            .HasDbFunction(methodInfo!)
            .HasTranslation(args => new SqlFunctionExpression(nameof(DatePart),
                    new[]
                    {
                        new SqlFragmentExpression(((SqlConstantExpression)args[0]).Value.ToString()),
                        args[1]
                    },
                    false,
                    new List<bool>() { false, false, false },
                    typeof(int?),
                    null
                )
            );
    }
}