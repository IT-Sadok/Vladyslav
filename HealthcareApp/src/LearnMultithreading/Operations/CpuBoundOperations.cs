namespace LearnMultithreading.Operations;

public static class CpuBoundOperations
{
    public static async Task<int> PerformComplexCalculation(int value)
    {
        return await Task.Run(() => 
        {
            // Simulate a complex CPU-bound calculation
            int result = Enumerable.Range(0, value * 1000000)
                .AsParallel()
                .WithDegreeOfParallelism( Environment.ProcessorCount)
                .Sum(i => i % 3);
            
            return result;
        });
    }
}