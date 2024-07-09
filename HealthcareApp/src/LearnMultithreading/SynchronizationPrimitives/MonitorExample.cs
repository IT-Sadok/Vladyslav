using Domain.Entities;

namespace LearnMultithreading.SynchronizationPrimitives;

public class MonitorExample
{
    private readonly object _lockObject = new object();
    private List<Schedule> _schedules = new List<Schedule>();

    public bool TryAddSchedule(Schedule schedule, int millisecondsTimeout)
    {
        if (Monitor.TryEnter(_lockObject, millisecondsTimeout))
        {
            try
            {
                _schedules.Add(schedule);
                return true;
            }
            finally
            {
                Monitor.Exit(_lockObject);
            }
        }
        else
        {
            return false; // Timeout occurred
        }
    }

    public List<Schedule> GetSchedules()
    {
        Monitor.Enter(_lockObject);
        try
        {
            return new List<Schedule>(_schedules);
        }
        finally
        {
            Monitor.Exit(_lockObject);
        }
    }
}