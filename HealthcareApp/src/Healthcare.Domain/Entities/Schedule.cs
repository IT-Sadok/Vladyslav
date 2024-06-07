namespace Domain.Entities;

public class Schedule
{
    public int ScheduleId { get; set; }
    
    public string DoctorId { get; set; }
    
    public ApplicationUser Doctor { get; set;  }
    
    public DateTime Date { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
}