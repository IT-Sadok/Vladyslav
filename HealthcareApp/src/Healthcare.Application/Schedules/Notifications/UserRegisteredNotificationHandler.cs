using Application.Abstractions;
using Domain.Constants;
using Domain.Entities;
using MediatR;

namespace Healthcare.Application.Schedules.Notifications;

public class UserRegisteredNotificationHandler : INotificationHandler<UserRegisteredNotification>
{
    private readonly IScheduleRepository _repository;

    public UserRegisteredNotificationHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        var workingHours = new List<Schedule>();
        var startTime = new TimeSpan(8, 0, 0);
        var endTime = new TimeSpan(16, 0, 0);

        for (int i = 0; i < 5; i++)
        {
            var date = DateTime.Today.AddDays(i - (int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            workingHours.Add(new Schedule
            {
                DoctorId = notification.UserId,
                DayOfWeek = date.DayOfWeek,
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            });
        }
        
        if(notification.Role == UserRolesConstants.Doctor)
            await _repository.CreateDefaultWorkingScheduleAsync(workingHours);
    }
}