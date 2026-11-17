using Microsoft.EntityFrameworkCore;

namespace IzpitSimulator.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<QuestionEntity> Questions => Set<QuestionEntity>();
    public DbSet<AnswerOptionEntity> AnswerOptions => Set<AnswerOptionEntity>();
    public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<QuestionEntity>(e =>
        {
            e.Property(x => x.Category).HasMaxLength(50).IsRequired();
            e.Property(x => x.Text).HasMaxLength(500).IsRequired();
            e.Property(x => x.Explanation).HasMaxLength(500).IsRequired();
            e.Property(x => x.Sign).HasMaxLength(30);
            e.HasMany(x => x.Options).WithOne(o => o.Question!).HasForeignKey(o => o.QuestionId).OnDelete(DeleteBehavior.Cascade);
        });
        b.Entity<AnswerOptionEntity>(e => e.Property(x => x.Text).HasMaxLength(300).IsRequired());
    }
}
