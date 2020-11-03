using System.Threading;

namespace Avalonia.Threading
{
    public interface IDispatcherImpl : IDispatcher
    {
        /// <summary>
        /// Runs the dispatcher's main loop.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token used to exit the main loop.
        /// </param>
        void MainLoop(CancellationToken cancellationToken);

        /// <summary>
        /// Runs continuations pushed on the loop.
        /// </summary>
        void RunJobs();

        /// <summary>
        /// Use this method to ensure that more prioritized tasks are executed
        /// </summary>
        /// <param name="minimumPriority"></param>
        void RunJobs(DispatcherPriority minimumPriority);

        /// <summary>
        /// This is needed for platform backends that don't have internal priority system (e. g. win32)
        /// To ensure that there are no jobs with higher priority
        /// </summary>
        /// <param name="currentPriority"></param>
        void EnsurePriority(DispatcherPriority currentPriority);

        /// <summary>
        /// Allows unit tests to change the platform threading interface.
        /// </summary>
        void UpdateServices();
    }
}
