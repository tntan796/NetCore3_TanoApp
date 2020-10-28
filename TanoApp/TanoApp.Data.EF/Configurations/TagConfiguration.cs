using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TanoApp.Data.EF.Extendsions;
using TanoApp.Data.Entities;

namespace TanoApp.Data.EntityFramwork.Configurations
{
    public class TagConfiguration : DbEntityConfiguration<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> entity)
        {
            //entity.Property(c => c.Id).HasMaxLength(50).IsRequired().HasColumnType("varchar");
        }
    }
}
