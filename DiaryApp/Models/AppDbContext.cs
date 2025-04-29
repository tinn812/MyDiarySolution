using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DiaryApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Diary> Diaries { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DiaryTag> DiaryTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 多對多關聯設定（複合主鍵）
            modelBuilder.Entity<DiaryTag>()
                .HasKey(dt => new { dt.DiaryId, dt.TagId });

            base.OnModelCreating(modelBuilder);

            // 自動把所有 DateTime 存成 UTC
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }
        }
    
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
            //optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        //}

    }
}
