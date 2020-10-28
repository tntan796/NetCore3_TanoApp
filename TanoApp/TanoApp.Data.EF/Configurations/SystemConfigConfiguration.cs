using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TanoApp.Data.EF.Extendsions;
using TanoApp.Data.Entities;

namespace TeduCoreApp.Data.EntityFramwork.Configurations
{
    class SystemConfigConfiguration : DbEntityConfiguration<SystemConfig>
    {
        public override void Configure(EntityTypeBuilder<SystemConfig> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            // etc.
        }
    }
}
