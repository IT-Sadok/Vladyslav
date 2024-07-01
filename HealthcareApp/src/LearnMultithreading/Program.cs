// See https://aka.ms/new-console-template for more information

namespace LearnMultithreading;

public class Program
{
    public static void Main(string[] args)
    {
        var obj = new object();
        List<string> names = new List<string>();
        var thread1 = new Thread(() =>
        {
            lock (obj)
            {
                names.Add("Denis");
            }
        });
        var thread2 = new Thread(() =>
        {
            lock (obj)
            {
                names.Add("Kolya");
            }
        });
        
        
        thread1.Start();
        thread2.Start();
    }
}