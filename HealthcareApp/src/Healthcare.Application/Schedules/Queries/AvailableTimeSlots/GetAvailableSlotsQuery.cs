using Healthcare.Application.DTOs.Schedule;
using MediatR;

namespace Healthcare.Application.Schedules.Queries.AvailableTimeSlots;

public record GetAvailableSlotsQuery(string DoctorId) : IRequest<Dictionary<string, List<TimeSlotDTO>>>;