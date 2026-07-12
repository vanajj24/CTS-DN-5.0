using System;

namespace SingletonPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Singleton Logger...");

            Logger logger1 = Logger.Instance;
            Logger logger2 = Logger.Instance;

            logger1.Log("First message");
            logger2.Log("Second message");

            Console.WriteLine($"Are both instances same? {ReferenceEquals(logger1, logger2)}");
        }
    }
}
