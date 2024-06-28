using Application.Abstractions.Dapper;
using Dapper;
using Healthcare.Infrastructure.Persistance;
using static Healthcare.Infrastructure.Persistance.SqlCommands;


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
        const string query = GetUsersCountByRole;

        return await sqlConnection.QueryFirstOrDefaultAsync<long>(query, new { RoleId = roleId });
    }

    public async Task<long> GetDoctorsAppointmentsCountAsync(string doctorId, DateTime? fromDate, DateTime? toDate)
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = GetDoctorsAppointmentsCountByTimeRange;
        return await sqlConnection.QueryFirstOrDefaultAsync<long>(query, new
        {
            DoctorId = doctorId,
            FromDate = fromDate,
            ToDate = toDate
        });
    }

    public async Task<long> GetDoctorsAppointmentsCountAsync(string doctorId)
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = GetDoctorAppointmentsCount;

        return await sqlConnection.QueryFirstOrDefaultAsync<long>(query, new { DoctorId = doctorId });
    }

    public async Task<float> GetMedianOfDurationTimeAsync()
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = GetMedianOfDurationTime;
        
        return await sqlConnection.QueryFirstOrDefaultAsync<float>(query);
    }

    public async Task<float> GetVarianceOfDurationTimeAsync()
    {
        using var sqlConnection = _dbConnectionFactory.CreateDbConnection();
        const string query = GetVarianceOfDurationTime;
        
        return await sqlConnection.QueryFirstOrDefaultAsync<float>(query);
    }
}