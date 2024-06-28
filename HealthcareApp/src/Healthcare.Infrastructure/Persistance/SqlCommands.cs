namespace Healthcare.Infrastructure.Persistance;

public static class SqlCommands
{
    public const string GetUsersCountByRole = """
                                              SELECT COUNT(*)
                                              FROM AspNetUserRoles
                                              WHERE RoleId = @RoleId
                                              GROUP BY RoleId
                                              """;

    public const string GetDoctorsAppointmentsCountByTimeRange = """
                                                                 SELECT COUNT(*), Appointments.DoctorId
                                                                 FROM Appointments
                                                                 WHERE AppointmentDate < @ToDate
                                                                     AND AppointmentDate > @FromDate
                                                                 GROUP BY Appointments.DoctorId
                                                                 HAVING DoctorId=@DoctorId;
                                                                 """;

    public const string GetDoctorAppointmentsCount = """
                                                     SELECT COUNT(*), Appointments.DoctorId
                                                     FROM Appointments
                                                     GROUP BY Appointments.DoctorId
                                                     HAVING DoctorId=@DoctorId;
                                                     """;

    public const string GetMedianOfDurationTime = """
                                                  SELECT 
                                                      DISTINCT PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY DurationMinutes) 
                                                      OVER () AS MedianValue
                                                  FROM Appointments;
                                                  """;

    public const string GetVarianceOfDurationTime = """
                                                    SELECT VAR(DurationMinutes) AS DurationVariance
                                                    FROM Appointments;
                                                    """;

    public const string UpsertAppointment = """
                                            MERGE INTO Appointments AS target
                                                       USING (SELECT @Id AS Id) AS source
                                                       ON (target.Id = source.Id)
                                                       WHEN MATCHED THEN 
                                                           UPDATE SET 
                                                               DoctorId = @DoctorId,
                                                               PatientId = @PatientId,
                                                               AppointmentDate = @AppointmentDate,
                                                               StartTime = @StartTime,
                                                               EndTime = @EndTime,
                                                               Status = @Status,
                                                               DurationMinutes = @DurationMinutes
                                                       WHEN NOT MATCHED THEN
                                                           INSERT (DoctorId, PatientId, AppointmentDate, StartTime, EndTime, Status, DurationMinutes)
                                                           VALUES (@DoctorId, @PatientId, @AppointmentDate, @StartTime, @EndTime, @Status, @DurationMinutes);
                                            """;
}