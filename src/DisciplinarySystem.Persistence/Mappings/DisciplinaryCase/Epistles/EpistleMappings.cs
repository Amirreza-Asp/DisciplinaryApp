using DisciplinarySystem.Domain.Epistles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Epistles
{
    public class EpistleMappings : IEntityTypeConfiguration<Epistle>
    {

        public void Configure ( EntityTypeBuilder<Epistle> builder )
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Subject);
            builder.Property(u => u.Type);
            builder.Property(u => u.CreateDate).HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.Description);

            builder.Property(u => u.UpdateDate).IsRequired(false);
            builder.Property(u => u.Reciver);
            builder.Property(u => u.Sender);


            builder.HasOne(u => u.Case)
                .WithMany(u => u.Epistles)
                .HasForeignKey(u => u.CaseId);
            builder.Property(u => u.CaseId).IsRequired(false);


            builder.HasOne(u => u.Complaint)
                .WithMany(u => u.Epistles)
                .HasForeignKey(u => u.ComplaintId);
            builder.Property(u => u.ComplaintId).IsRequired(false);
        }
    }
}
