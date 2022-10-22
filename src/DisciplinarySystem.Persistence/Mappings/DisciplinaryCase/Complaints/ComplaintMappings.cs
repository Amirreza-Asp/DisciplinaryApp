using DisciplinarySystem.Domain.Complaints;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Complaints
{
    public class ComplaintMappings
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Result);

            builder.Property(x => x.Title);
            builder.Property(x => x.Description);

            builder.HasOne(x => x.Plaintiff).
                WithOne(x => x.Complaint)
                .HasForeignKey<Complaint>(x => x.PlaintiffId);


            builder.HasOne(x => x.Complaining)
                .WithOne(x => x.Complaint)
                .HasForeignKey<Complaint>(x => x.ComplainingId);

        }
    }
}
