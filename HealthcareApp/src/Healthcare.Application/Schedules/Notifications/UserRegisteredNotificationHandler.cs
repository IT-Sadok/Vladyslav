using Application.Abstractions;
using Domain.Constants;
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
        switch (notification.Role)
        {
            case UserRolesConstants.Doctor:
                await _repository.CreateDefaultWorkingScheduleAsync(notification.UserId);
                break;
        }
    }
}