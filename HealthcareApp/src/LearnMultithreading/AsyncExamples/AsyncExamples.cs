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

    public static async Task ExampleConfigureAwait()
    {
        await Task.Delay(2000).ConfigureAwait(false);
        
        Console.WriteLine("Task completed.");
    }

    public static async Task ExampleTaskFactory()
    {
        var longRunningTask = Task.Factory.StartNew(() =>
        {
            var collection = Enumerable.Range(-100_000_000, 100_000_000);
            foreach (var _ in collection)
            {
            }
        }, TaskCreationOptions.LongRunning);

        await longRunningTask;

        Console.WriteLine("Long-running task completed.");
    }

    public static async Task ExampleContinueWith()
    {
        var task = Task.Delay(1000);

        var continuation = task.ContinueWith(t =>
        {
            Console.WriteLine("Continuation task completed.");
        });

        await continuation;
    }
}