using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Team;

namespace Streetcode.DAL.Configurations;

public class TeamMemberPositionsMap : IEntityTypeConfiguration<TeamMemberPositions>
{
    public void Configure(EntityTypeBuilder<TeamMemberPositions> builder)
    {
        builder
            .HasKey(nameof(TeamMemberPositions.TeamMemberId), nameof(TeamMemberPositions.PositionsId));
    }
}