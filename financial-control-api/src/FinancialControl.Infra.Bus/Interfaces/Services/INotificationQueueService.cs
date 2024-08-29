using FinancialControl.Infra.Bus.Dtos.Notifications;

namespace FinancialControl.Infra.Bus.Interfaces.Services
{
    public interface INotificationQueueService
    {
        void EnqueueSendingEmail(EmailParametersDTO emailParameters);
    }
}
