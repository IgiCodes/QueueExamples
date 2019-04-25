using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPLQueue
{
    public class Queue
    {
        /// <summary>
        ///     The internal job queue.
        /// </summary>
        private readonly BroadcastBlock<IJob> _queue = new BroadcastBlock<IJob>(job => job);

        /// <summary>
        ///     Attach a handler to the queue.
        /// </summary>
        /// <param name="handler">The handler to attach.</param>
        /// <param name="numberOfThreads">The number of threads to assign to the handler.</param>
        /// <typeparam name="T">Job type to assign the handler to.</typeparam>
        public void RegisterConsumer<T>(Action<T> handler, int numberOfThreads = 1) where T : IJob
        {
            void Action(IJob job)
            {
                handler((T) job);
            }

            var actionBlock = new ActionBlock<IJob>(
                Action,
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = numberOfThreads
                });

            _queue.LinkTo(actionBlock, job => job is T);
        }

        /// <summary>
        ///     Add a job to the queue.
        /// </summary>
        /// <param name="job">The job to add to the queue.</param>
        /// <returns></returns>
        public async Task Enqueue(IJob job)
        {
            Console.WriteLine($"Job queuing: {job}");
            await _queue.SendAsync(job);
        }
    }
}