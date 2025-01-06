using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Entities.Users.Expertise;

namespace Streetcode.DAL.Configurations;

public class ExpertiseMap : IEntityTypeConfiguration<Expertise>
{
    public void Configure(EntityTypeBuilder<Expertise> builder)
    {
        //builder
        //    .HasMany(x => x.Positions)
        //    .WithMany(x => x.TeamMembers)
        //    .UsingEntity<TeamMemberPositions>(
        //        tp => tp.HasOne(x => x.Positions)
        //            .WithMany()
        //            .HasForeignKey(x => x.PositionsId),
        //        tp => tp.HasOne(x => x.TeamMember)
        //            .WithMany()
        //            .HasForeignKey(x => x.TeamMemberId));


        builder
            .HasMany(x => x.Users)
            .WithMany(x => x.Expertises)
            .UsingEntity<UserExpertise>(
            ue => ue
                            .HasOne(x => x.User)
                            .WithMany()
                            .HasForeignKey(x => x.UserId)
                            .OnDelete(DeleteBehavior.Cascade), 
                        ue => ue
                            .HasOne(x => x.Expertise)
                            .WithMany()
                            .HasForeignKey(x => x.ExpertiseId)
                            .OnDelete(DeleteBehavior.Cascade)
            );
    }
}