using FinancialControl.Domain.Entities.Shared;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Domain.Entities
{
    public class ValidationCode : EntityBase
    {
        public required string Code { get; set; }
        public int UserId { get; set; }
        public ValidationCodePurpose Purpose { get; set; }        
        public DateTime Expiration { get; set; }

        public User? User { get; set; }

    }
}
