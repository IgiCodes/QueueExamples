using System;
using System.Threading;

namespace Queue
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("App Started!");
            Start();
            Console.ReadLine();
        }

        private static void Start()
        {
            var queue = new Queue(1);

            new Thread(async () =>
            {
                await queue.Enqueue(new JobA("Thread 1 Job 1"));
                await queue.Enqueue(new JobB("Thread 1 Job 2"));
                await queue.Enqueue(new JobA("Thread 1 Job 3"));
                await queue.Enqueue(new JobA("Thread 1 Job 4"));
            }).Start();

            new Thread(async () =>
            {
                await queue.Enqueue(new JobB("Thread 2 Job 1"));
                await queue.Enqueue(new JobB("Thread 2 Job 2"));
                await queue.Enqueue(new JobA("Thread 2 Job 3"));
            }).Start();
        }
    }
}