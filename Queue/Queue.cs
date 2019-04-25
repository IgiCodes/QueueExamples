using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Queue
{
    public class Queue
    {
        /// <summary>
        ///     The internal job queue.
        /// </summary>
        private readonly BlockingCollection<IJob> _queue = new BlockingCollection<IJob>();

        /// <summary>
        ///     Dictionary of queue processing threads keyed by their respective cancellation token.
        /// </summary>
        private readonly ConcurrentDictionary<CancellationToken, Thread>
            _threads = new ConcurrentDictionary<CancellationToken, Thread>();

        /// <summary>
        ///     Queue constructor
        /// </summary>
        /// <param name="numberOfThreads">The number of concurrent threads to processes the queue with.</param>
        public Queue(int numberOfThreads)
        {
            for (var i = 0; i < numberOfThreads; i++)
            {
                var cancellationToken = new CancellationToken();
                var thread = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    foreach (var job in _queue.GetConsumingEnumerable(cancellationToken))
                        try
                        {
                            job.Invoke();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                });

                _threads.TryAdd(cancellationToken, thread);

                thread.Start();
            }
        }

        /// <summary>
        ///     Stop the queue and prevent adding further jobs.
        /// </summary>
        public void Stop()
        {
            _queue.CompleteAdding();
        }

        /// <summary>
        ///     Add a job to the queue.
        /// </summary>
        /// <param name="job">The job to add to the queue.</param>
        /// <param name="timeout">The timeout for adding a job to the queue.</param>
        /// <returns></returns>
        public async Task Enqueue(IJob job, int timeout = 1000)
        {
            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine($"Job queuing: {job}");
                    _queue.TryAdd(job, TimeSpan.FromMilliseconds(timeout));
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}