namespace Healthcare.Application.DTOs.Schedule;

public record TimeSlotDTO (TimeSpan StartTime, TimeSpan EndTime, bool IsAvailable);