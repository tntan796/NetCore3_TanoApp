﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TanoApp.Data.EF.Extendsions;
using TanoApp.Data.Entities;

namespace TeduCoreApp.Data.EF.Configurations
{
    public class BlogTagConfiguration : DbEntityConfiguration<BlogTag>
    {
        public override void Configure(EntityTypeBuilder<BlogTag> entity)
        {
            //entity.Property(c => c.TagId).HasMaxLength(50).IsRequired()
            //.IsUnicode(false).HasMaxLength(50);
            // etc.
        }
    }
}