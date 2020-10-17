using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Infrastructure.Contracts
{
    public interface IReminderGateway
    {
        Task SendPostToApiReminder(MobileDataPackage mobileDataPackage);
    }
}