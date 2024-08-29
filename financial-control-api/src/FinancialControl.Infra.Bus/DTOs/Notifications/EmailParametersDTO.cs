using FinancialControl.Infra.Bus.Enums;

namespace FinancialControl.Infra.Bus.Dtos.Notifications
{
    public record EmailParametersDTO(
        string UserName,
        string UserEmail,
        string Code,
        EmailPurpose EmailPurpose
    );
    
}
