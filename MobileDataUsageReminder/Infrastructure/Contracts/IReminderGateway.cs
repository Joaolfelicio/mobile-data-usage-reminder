using System.Threading.Tasks;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Infrastructure.Contracts
{
    public interface IReminderGateway
    {
        /// <summary>
        /// Sends the post to API reminder.
        /// </summary>
        /// <param name="mobileData">The mobile data package.</param>
        /// <returns></returns>
        Task SendPostToApiReminder(MobileData mobileData);
    }
}