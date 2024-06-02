using Healthcare.Application.DTOs.Schedule;
using MediatR;

namespace Healthcare.Application.Schedules.Querries.AvailableTimeSlots;

public record GetAvailableSlotsQuery(string DoctorId, DateTime Date) : IRequest<List<TimeSlotDTO>>;