using System.Diagnostics;
using Domain.Entities;
using LearnMultithreading.AsyncExamples;
using LearnMultithreading.Operations;
using LearnMultithreading.SynchronizationPrimitives;
using static LearnMultithreading.AsyncExamples.AsyncExamples;

// I/O-Bound operation
var fetchedData = await IOBoundOperations.FetchData();
Console.WriteLine("I/O-Bound operation: " + fetchedData);

// CPU-Bound operation
var calculationsResult = await CpuBoundOperations.PerformComplexCalculation(99);
Console.WriteLine("CPU-Bound operation: " + calculationsResult);

var threads = new List<Thread>();
var tasks = new List<Task>();

// Lock example
var lockExample = new LockExample();
threads.Clear();
for (int i = 0; i < 10; i++)
{
    var appointment = new Appointment
        { Id = i, DoctorId = "Doc3", PatientId = $"Pat{i}", AppointmentDate = DateTime.Now };
    threads.Add(new Thread(() => lockExample.AddAppointment(appointment)));
    threads[i].Start();
}

foreach (var thread in threads)
{
    thread.Join();
}

Console.WriteLine($"LockExample: {lockExample.GetAppointments().Count} appointments added.");

// ReaderWriterLockSlim example
var readerWriterLockSlimExample = new ReaderWriterLockSlimExample();
threads.Clear();
for (int i = 0; i < 3; i++)
{
    var appointment = new Appointment
        { Id = i, DoctorId = "Doc3", PatientId = $"Pat{i}", AppointmentDate = DateTime.Now };
    threads.Add(new Thread(() => readerWriterLockSlimExample.AddAppointment(appointment)));
    threads[i].Start();
}

foreach (var thread in threads)
{
    thread.Join();
}

Console.WriteLine($"ReaderWriterLockSlimExample: {readerWriterLockSlimExample.GetAppointments().Count} appointments added.");

// Interlocked example
var interlockedExample = new InterlockedExample();
threads.Clear();
for (int i = 0; i < 10; i++)
{
    threads.Add(new Thread(interlockedExample.IncrementAppointmentCount));
    threads[i].Start();
}

foreach (var thread in threads)
{
    thread.Join();
}

Console.WriteLine($"InterlockedExample: {interlockedExample.GetAppointmentCount()} increments.");

// Monitor example
var monitorExample = new MonitorExample();
tasks.Clear();
for (int i = 0; i < 10; i++)
{
    var schedule = new Schedule
    {
        Id = i, DoctorId = "Doc4", Date = DateTime.Now.AddDays(i), DayOfWeek = (DayOfWeek)(i % 7),
        StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(16, 0, 0)
    };
    var task = Task.Run(() =>
    {
        Console.WriteLine(monitorExample.TryAddSchedule(schedule, 1000)
            ? $"MonitorExample: Schedule {i} added."
            : $"MonitorExample: Schedule {i} timeout.");
    });
    tasks.Add(task);
}

await Task.WhenAll(tasks);
Console.WriteLine($"MonitorExample: {monitorExample.GetSchedules().Count} schedules added.");

// Semaphore example
var semaphoreExample = new SemaphoreExample();
tasks.Clear();
for (int i = 0; i < 10; i++)
{
    var appointment = new Appointment
        { Id = i, DoctorId = "Doc1", PatientId = $"Pat{i}", AppointmentDate = DateTime.Now };
    tasks.Add(semaphoreExample.AddAppointmentAsync(appointment));
}

await Task.WhenAll(tasks);
Console.WriteLine($"SemaphoreExample: {semaphoreExample.GetAppointments().Count} appointments added.");


// SpinWait example
var spinWaitExample = new SpinWaitExample();
var spinWaitTask = Task.Run(() => spinWaitExample.WaitForAppointments());
spinWaitExample.AddAppointment(new Appointment
    { Id = 1, DoctorId = "Doc2", PatientId = "Pat1", AppointmentDate = DateTime.Now });
await spinWaitTask;
Console.WriteLine($"SpinWaitExample: {spinWaitExample.GetAppointments().Count} appointment added.");

// Parallel invocation
Parallel.Invoke(() =>
    {
        Console.WriteLine($"The task is in process: {Task.CurrentId}");
        Thread.Sleep(3000);
        Console.WriteLine($"Completed!");
    },
    () =>
    {
        Console.WriteLine($"The task is in process: {Task.CurrentId}");
        Thread.Sleep(3000);
        Console.WriteLine($"Completed!");
    },
    () =>
    {
        Console.WriteLine($"The task is in process: {Task.CurrentId}");
        Thread.Sleep(3000);
        Console.WriteLine($"Result {(10 * 56) + 900}");
    });

Parallel.For(1, 6, i => { Console.WriteLine(i * i); });

//PLINQ:
var stopwatch = Stopwatch.StartNew();
var source = Enumerable.Range(0, 10)
    .AsParallel()
    .Select((x) =>
    {
        for (int i = 0; i < 150_000_000; i++)
        {
            x += i;
        }

        return x;
    });

foreach (var _ in source)
    stopwatch.Stop();
Console.WriteLine("Time required for calculation: " + stopwatch.ElapsedMilliseconds);

//WhenAny use case:
await ExampleTaskWhenAny();

//ConfigureAwait use case:
await ExampleConfigureAwait(false); // In this case 2 different threads are used

await ExampleConfigureAwait(true); // And here it is using only one thread


//TaskFactory use case:
await ExampleTaskFactory();

//ContinueWith use case:
await ExampleContinueWith();

//ValueTask use case example
var timeSlots = await ValueTaskExample.FetchData();
Console.WriteLine($"Doctor has {timeSlots.Keys.Count} working days");

// Calling Deadlock:
object lock1 = new object();
object lock2 = new object();

Thread thread1 = new Thread(ExecuteThread1);
Thread thread2 = new Thread(ExecuteThread2);

thread1.Start();
thread2.Start();

thread1.Join();
thread2.Join();


void ExecuteThread1()
{
    lock (lock1)
    {
        Console.WriteLine("Thread 1: Holding lock1...");
        Thread.Sleep(100);

        Console.WriteLine("Thread 1: Waiting for lock2 to release data...");
        lock (lock2)
        {
            Console.WriteLine("Thread 1: Acquired lock2!");
        }
    }
}

void ExecuteThread2()
{
    lock (lock2)
    {
        Console.WriteLine("Thread 2: Holding lock2...");
        Thread.Sleep(100);

        Console.WriteLine("Thread 2: Waiting for lock1 to release data...");
        lock (lock1)
        {
            Console.WriteLine("Thread 2: Acquired lock1!");
        }
    }
}

Console.Read();