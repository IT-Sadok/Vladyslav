using Domain.Constants;

namespace Application.Abstractions.Dapper;

public interface IMetricsRepository
{
    Task<long> GetUsersCountByRoleAsync(int roleId);
    
    Task<long> GetDoctorsAppointmentsCountAsync(string doctorId, DateTime? fromDate, DateTime? toDate);
    
    Task<long> GetDoctorsAppointmentsCountAsync(string doctorId);

    Task<float> GetMedianOfDurationTimeAsync();
    
    Task<float> GetVarianceOfDurationTimeAsync();
}