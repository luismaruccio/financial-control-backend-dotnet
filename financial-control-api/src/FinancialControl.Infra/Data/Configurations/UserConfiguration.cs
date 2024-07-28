using FinancialControl.Domain.Entities;
using FinancialControl.Infra.Data.Configurations.Shareds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialControl.Infra.Data.Configurations
{
    public class UserConfiguration : EntityBaseConfiguration<User>
    {
        protected override string TableName => "Users";

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.EmailVerified).IsRequired().HasDefaultValue(false);
        }
    }
}
