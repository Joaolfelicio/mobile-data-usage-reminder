using System.Threading.Tasks;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Infrastructure.Contracts
{
    public interface IReminderGateway
    {
        /// <summary>
        /// Sends the post to API reminder.
        /// </summary>
        /// <param name="mobileDataPackage">The mobile data package.</param>
        /// <returns></returns>
        Task SendPostToApiReminder(MobileDataPackage mobileDataPackage);
    }
}