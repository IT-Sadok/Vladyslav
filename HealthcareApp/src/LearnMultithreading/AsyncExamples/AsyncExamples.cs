namespace LearnMultithreading.AsyncExamples;

public class AsyncExamples
{
    public static async Task ExampleTaskWhenAny()
    {
        var task1 = Task.Delay(1000);
        var task2 = Task.Delay(2000);
        var task3 = Task.Delay(3000);

        var firstCompletedTask = await Task.WhenAny(task1, task2, task3);

        Console.WriteLine("First task completed.");
    }

    public static async Task ExampleConfigureAwait(bool marker)
    {
        Console.WriteLine($"Startint the task on thread: {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(2000).ConfigureAwait(marker);

        Console.WriteLine($"Task completed on thread: {Thread.CurrentThread.ManagedThreadId}");
    }

    public static async Task ExampleTaskFactory()
    {
        /*
         * Task.Run is just a simplified version of Task.Factory.StartNew with passing default params.
         * Task.Factory.StartNew enables us to set a Task for instance as a long-running
         */

        var longRunningTask = Task.Factory.StartNew(() =>
        {
            var collection = Enumerable.Range(-100_000_000, 100_000_000);
            foreach (var _ in collection)
            {
            }
        }, TaskCreationOptions.LongRunning);

        await longRunningTask;

        Console.WriteLine("Long-running task completed.");
        /*
         * Specifying long-running task help us to give a hint for task scheduler to provide separate
         * thread and avoid blocking the execution of other tasks.
         */
    }

    public static async Task ExampleContinueWith()
    {
        var task = Task.Delay(1000);

        var continuation = task.ContinueWith(t => { Console.WriteLine("Continuation task completed."); });

        await continuation;
    }
}