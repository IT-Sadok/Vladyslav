namespace LearnMultithreading.SynchronizationPrimitives;

public class InterlockedExample
{
    private int _appointmentCount = 0;

    public void IncrementAppointmentCount()
    {
        Interlocked.Increment(ref _appointmentCount);
    }

    public int GetAppointmentCount()
    {
       return Interlocked.CompareExchange(ref _appointmentCount, 0, 0);
    }
}