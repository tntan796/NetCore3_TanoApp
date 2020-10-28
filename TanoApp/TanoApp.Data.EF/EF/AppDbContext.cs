using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using TanoApp.Data.EF.Configurations;
using TanoApp.Data.EF.Extendsions;
using TanoApp.Data.Entities;
using TanoApp.Data.EntityFramwork.Configurations;
using TanoApp.Data.Interfaces;
using TeduCoreApp.Data.EF.Configurations;
using TeduCoreApp.Data.EntityFramwork.Configurations;

namespace TanoApp.Data.EF.EF
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Advertisement> Advertisements { set; get; }
        public DbSet<AdvertisementPage> AdvertisementPages { set; get; }
        public DbSet<AdvertisementPosition> AdvertisementPositions { set; get; }
        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }
        public DbSet<AppUser> AppUsers { set; get; }
        public DbSet<AppRole> AppRoles { set; get; }
        public DbSet<Bill> Bills { set; get; }
        public DbSet<BillDetail> BillDetails { set; get; }
        public DbSet<Blog> Blogs { set; get; }
        public DbSet<BlogTag> BlogTags { set; get; }
        public DbSet<Color> Colors { set; get; }
        public DbSet<Contact> Contacts { set; get; }
        public DbSet<FeedBack> Feedbacks { set; get; }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Function> Functions { set; get; }
        public DbSet<Language> Languages { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }
        public DbSet<Tag> Tags { set; get; }
        public DbSet<WholePrice> WholePrices { set; get; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityRole<string>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });

            builder.AddConfiguration(new AdvertistmentPositionConfiguration());
            builder.AddConfiguration(new BlogTagConfiguration());
            builder.AddConfiguration(new ContactDetailConfiguration());
            builder.AddConfiguration(new FooterConfiguration());
            builder.AddConfiguration(new FunctionConfiguration());
            builder.AddConfiguration(new PageConfiguration());
            builder.AddConfiguration(new ProductTagConfiguration());
            builder.AddConfiguration(new SystemConfigConfiguration());
            builder.AddConfiguration(new TagConfiguration());

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var modified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                var updateOrAddItem = item as IDateTracking;
                if (updateOrAddItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        updateOrAddItem.DateCreated = DateTime.Now;
                    }
                    updateOrAddItem.DateModified = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
}
