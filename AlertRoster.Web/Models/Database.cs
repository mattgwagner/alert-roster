using Microsoft.EntityFrameworkCore;

namespace AlertRoster.Web.Models
{
    public class Database : DbContext
    {
        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<MemberGroup> MemberGroups { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Data.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Group>()
                .HasIndex(group => group.DisplayName)
                .IsUnique();

            builder
                .Entity<Member>()
                .HasIndex(member => member.PhoneNumber)
                .IsUnique();

            builder
                .Entity<MemberGroup>()
                .HasKey(_ => new { _.MemberId, _.GroupId });

            builder
                .Entity<MemberGroup>()
                .HasOne(_ => _.Group)
                .WithMany(_ => _.Members)
                .HasForeignKey(_ => _.GroupId);

            builder
                .Entity<MemberGroup>()
                .HasOne(_ => _.Member)
                .WithMany(_ => _.Groups)
                .HasForeignKey(_ => _.MemberId);
        }

        public static void Init(Database db)
        {
            db.Database.Migrate();
        }
    }
}