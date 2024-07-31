using Domain.Entities;

namespace Domain.Constants;

public static class DefaultWorkingHours
{
    public static IReadOnlyList<Schedule> CreateWorkingHours(string doctorId)
    {
        var workingHours = new List<Schedule>();
        var startTime = new TimeSpan(8, 0, 0);
        var endTime = new TimeSpan(16, 0, 0);
        var currentDay = DateTime.Today;
        var startOfWeek = currentDay.AddDays(-((int)currentDay.DayOfWeek - (int)DayOfWeek.Monday));
        startOfWeek = startOfWeek.Date; // Just to make sure we are working with dates only

        for (int i = 0; i < 5; i++)
        {
            var date = startOfWeek.AddDays(i);
            workingHours.Add(new Schedule
            {
                DoctorId = doctorId,
                DayOfWeek = date.DayOfWeek,
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            });
        }

        return  workingHours.AsReadOnly();
    }
}