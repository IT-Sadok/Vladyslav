using Domain.Constants;

namespace Application.Abstractions.Dapper;

public interface IMetricsRepository
{
    Task<long> GetUsersCountByRoleAsync(int roleId);
    
    Task<List<object>> GetDoctorsWithAppointments();
    
    Task<long> GetDoctorAppointmentsCount(string doctorId);

    Task<float> GetMedianOfDurationTime();
    
    Task<float> GetVarianceOfDurationTime();
}