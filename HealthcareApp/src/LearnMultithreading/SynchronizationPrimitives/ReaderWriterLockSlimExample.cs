using Domain.Entities;

namespace LearnMultithreading.SynchronizationPrimitives
{
    public class ReaderWriterLockSlimExample
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private List<Appointment> _appointments = new List<Appointment>();

        public void AddAppointment(Appointment appointment)
        {
            _lock.EnterWriteLock();
            try
            {
                _appointments.Add(appointment);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public List<Appointment> GetAppointments()
        {
            _lock.EnterReadLock();
            try
            {
                return new List<Appointment>(_appointments);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}