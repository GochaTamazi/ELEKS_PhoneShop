using Microsoft.EntityFrameworkCore;
using Models.Entities.PhoneShop;
using Models.Entities.RemoteApi;

#nullable disable
namespace Database
{
    public partial class MasterContext : DbContext
    {
        public MasterContext()
        {
        }

        public MasterContext(DbContextOptions<MasterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Models.Entities.RemoteApi.Phone> PhonesRemoteApi { get; set; }
        public virtual DbSet<Specification> Specifications { get; set; }

        public virtual DbSet<Models.Entities.PhoneShop.Phone> PhonesPhoneShop { get; set; }
        public virtual DbSet<PriceSubscriber> PriceSubscribers { get; set; }
        public virtual DbSet<StockSubscriber> StockSubscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Brands_pk")
                    .IsClustered(false);

                entity.ToTable("Brands", "RemoteApi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Slug)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("slug");
            });

            modelBuilder.Entity<Models.Entities.PhoneShop.Phone>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Phones_pk")
                    .IsClustered(false);

                entity.ToTable("Phones", "PhoneShop");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Hided).HasColumnName("hided");

                entity.Property(e => e.PhoneId).HasColumnName("Phone_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Stock).HasColumnName("stock");
            });

            modelBuilder.Entity<Models.Entities.RemoteApi.Phone>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Phones_pk")
                    .IsClustered(false);

                entity.ToTable("Phones", "RemoteApi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("Brand_id");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Slug)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("slug");
            });

            modelBuilder.Entity<PriceSubscriber>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PriceSubscribers_pk")
                    .IsClustered(false);

                entity.ToTable("PriceSubscribers", "PhoneShop");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.PhoneId).HasColumnName("Phone_id");
            });

            modelBuilder.Entity<Specification>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Specifications_pk")
                    .IsClustered(false);

                entity.ToTable("Specifications", "RemoteApi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("Brand_id");

                entity.Property(e => e.Dimension)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("dimension");

                entity.Property(e => e.Images)
                    .IsUnicode(false)
                    .HasColumnName("images");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Os)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("os");

                entity.Property(e => e.PhoneId).HasColumnName("Phone_id");

                entity.Property(e => e.ReleaseDate)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("release_date");

                entity.Property(e => e.Specifications)
                    .IsUnicode(false)
                    .HasColumnName("specifications");

                entity.Property(e => e.Storage)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("storage");

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("thumbnail");
            });

            modelBuilder.Entity<StockSubscriber>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("StockSubscribers_pk")
                    .IsClustered(false);

                entity.ToTable("StockSubscribers", "PhoneShop");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.PhoneId).HasColumnName("Phone_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}