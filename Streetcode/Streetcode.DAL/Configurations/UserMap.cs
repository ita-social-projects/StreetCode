using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.DAL.Configurations;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasOne(u => u.Avatar)
            .WithOne(i => i.User)
            .HasForeignKey<Image>(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}