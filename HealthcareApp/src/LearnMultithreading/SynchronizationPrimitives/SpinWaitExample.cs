using Domain.Entities;

namespace LearnMultithreading.SynchronizationPrimitives;

public class SpinWaitExample
{
    private volatile bool _hasAppointments = false;
    private List<Appointment> _appointments = new List<Appointment>();

    public void AddAppointment(Appointment appointment)
    {
        _appointments.Add(appointment);
        _hasAppointments = true;
    }

    public void WaitForAppointments()
    {
        var spinWait = new SpinWait();
        while (!_hasAppointments)
        {
            spinWait.SpinOnce(); // Perform a spin wait
        }
    }

    public List<Appointment> GetAppointments()
    {
        return [.._appointments];
    }
}