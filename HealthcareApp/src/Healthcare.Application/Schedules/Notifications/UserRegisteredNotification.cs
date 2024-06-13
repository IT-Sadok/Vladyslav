using MediatR;

namespace Healthcare.Application.Schedules.Notifications;

public record UserRegisteredNotification(string Role, string UserId) : INotification;