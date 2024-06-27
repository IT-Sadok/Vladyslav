using System.Text.RegularExpressions;
using Application.Abstractions.Dapper;
using Dapper;
using Healthcare.Infrastructure.Persistance;

namespace Infrastructure.Repository.Dapper;

public class MetricsRepository : IMetricsRepository
{
    private readonly SqlConnectionFactory _dbConnectionFactory;

    public MetricsRepository(SqlConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<long> GetUsersCountByRoleAsync(int roleId)
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = """
                             SELECT COUNT(*)
                             FROM AspNetUserRoles
                             WHERE RoleId = @RoleId
                             GROUP BY RoleId
                             """;

        return await sqlConnection.QueryFirstOrDefaultAsync<long>(query, new { RoleId = roleId });
    }

    public async Task<List<object>> GetDoctorsWithAppointments()
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = """
                             SELECT AspNetUsers.FirstName, AspNetUsers.LastName, Appointments.Id AS AppointmentId, Appointments.AppointmentDate
                             FROM Appointments
                             INNER JOIN AspNetUsers ON AspNetUsers.Id = Appointments.DoctorId;
                             """;
        var result = await sqlConnection.QueryAsync<object>(query);
        return result.ToList();
    }

    public async Task<long> GetDoctorAppointmentsCount(string doctorId)
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = """
                             SELECT COUNT(*), Appointments.DoctorId
                             FROM Appointments
                             GROUP BY Appointments.DoctorId
                             HAVING DoctorId=@DoctorId;
                             """;

        return await sqlConnection.QueryFirstOrDefaultAsync<long>(query, new { DoctorId = doctorId });
    }

    public async Task<float> GetMedianOfDurationTime()
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = """
                             SELECT 
                                 DISTINCT PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY DurationMinutes) 
                                 OVER () AS MedianValue
                             FROM Appointments;
                             """;
        return await sqlConnection.QueryFirstOrDefaultAsync<float>(query);
    }

    public async Task<float> GetVarianceOfDurationTime()
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = """
                             SELECT VAR(DurationMinutes) AS DurationVariance
                             FROM Appointments;
                             """;
        return await sqlConnection.QueryFirstOrDefaultAsync<float>(query);
    }
}