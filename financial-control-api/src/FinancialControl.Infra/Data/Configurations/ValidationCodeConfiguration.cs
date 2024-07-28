using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infra.Data.Configurations.Shareds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialControl.Infra.Data.Configurations
{
    public class ValidationCodeConfiguration : EntityBaseConfiguration<ValidationCode>
    {
        protected override string TableName => "ValidationCodes";

        public override void Configure(EntityTypeBuilder<ValidationCode> builder)
        {
            base.Configure(builder);

            builder.HasKey(e => new { e.Id, e.UserId });

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Code).IsRequired().HasMaxLength(9);
            builder.Property(e => e.Purpose).IsRequired().HasConversion(
                v => v.ToString(),
                v => (ValidationCodePurpose)Enum.Parse(typeof(ValidationCodePurpose), v));
            builder.Property(e => e.Expiration).IsRequired();

            builder.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
