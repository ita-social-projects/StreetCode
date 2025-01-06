using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Entities.Users.Expertise;

namespace Streetcode.DAL.Configurations;

public class ExpertiseMap : IEntityTypeConfiguration<Expertise>
{
    public void Configure(EntityTypeBuilder<Expertise> builder)
    {
#pragma warning disable SA1028
        builder
            .HasMany(x => x.Users)
            .WithMany(x => x.Expertises)
            .UsingEntity<UserExpertise>(
    ue => ue.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade), 
    ue => ue
                    .HasOne(x => x.Expertise)
                    .WithMany()
                    .HasForeignKey(x => x.ExpertiseId)
                    .OnDelete(DeleteBehavior.Cascade))
            .ToTable("user_expertise", "users");
    }
}