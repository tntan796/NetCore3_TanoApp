using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TanoApp.Data.EF.Extendsions;
using TanoApp.Data.Entities;

namespace TeduCoreApp.Data.EntityFramwork.Configurations
{
    public class PageConfiguration : DbEntityConfiguration<Page>
    {
        public override void Configure(EntityTypeBuilder<Page> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            // etc.
        }
    }
}
