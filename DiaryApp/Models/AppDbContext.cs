using Microsoft.EntityFrameworkCore;

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
        }
    }
}
