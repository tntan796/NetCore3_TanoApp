using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TanoApp.Data.EF.Extendsions;
using TanoApp.Data.Entities;

namespace TeduCoreApp.Data.EntityFramwork.Configurations
{
    public class ProductTagConfiguration : DbEntityConfiguration<ProductTag>
    {
        public override void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            //entity.Property(c => c.TagId).HasMaxLength(50).IsRequired()
            //.HasMaxLength(50).IsUnicode(false);
            // etc.
        }
    }
}
