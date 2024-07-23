using FinancialControl.Domain.Entities.Shared;

namespace FinancialControl.Domain.Entities
{
    public class User : EntityBase
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public bool EmailVerified { get; set; }
    }
}
