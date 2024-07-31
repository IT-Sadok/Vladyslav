using Domain.Entities;

namespace LearnMultithreading.SynchronizationPrimitives;

public class LockExample
{
    private readonly object _lockObject = new object();
    private List<Appointment> _appointments = new List<Appointment>();

    public void AddAppointment(Appointment appointment)
    {
        lock (_lockObject)
        {
            _appointments.Add(appointment);
        }
    }

    public List<Appointment> GetAppointments()
    {
        lock (_lockObject)
        {
            return new List<Appointment>(_appointments);
        }
    }
}