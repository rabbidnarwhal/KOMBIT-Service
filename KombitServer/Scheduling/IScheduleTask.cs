using System.Threading;
using System.Threading.Tasks;

namespace KombitServer.ScheduleTask
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}