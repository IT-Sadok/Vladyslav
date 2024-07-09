using Domain.Entities;

namespace LearnMultithreading.SynchronizationPrimitives;

public class SemaphoreExample
{
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(3); // Limit to 3 concurrent threads
    private List<Appointment> _appointments = new List<Appointment>();

    public async Task AddAppointmentAsync(Appointment appointment)
    {
        await Semaphore.WaitAsync();
        try
        {
            // Simulate work
            await Task.Delay(1000);
            _appointments.Add(appointment);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public List<Appointment> GetAppointments()
    {
        return new List<Appointment>(_appointments);
    }
}