namespace LearnMultithreading.Operations;

public static class CpuBoundOperations
{
    public static async Task<int> PerformComplexCalculation(int value)
    {
        return await Task.Run(() => 
        {
            // Simulate a complex CPU-bound calculation
            int result = 0;
            for (int i = 0; i < value * 1000000; i++)
            {
                result += i % 3;
            }
            return result;
        });
    }
}