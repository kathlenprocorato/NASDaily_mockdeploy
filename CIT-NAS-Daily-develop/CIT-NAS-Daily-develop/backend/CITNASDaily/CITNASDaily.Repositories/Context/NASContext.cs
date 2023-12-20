using CITNASDaily.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CITNASDaily.Repositories.Context
{
    public class NASContext : DbContext
    {
        public NASContext(DbContextOptions <NASContext> options) : base(options) { }
        //tables sa db
		public DbSet<NAS> NAS { get; set; }
		public DbSet<Office> Offices { get; set; }
		public DbSet<Superior> Superiors { get; set; }
		public DbSet<SuperiorEvaluationRating> SuperiorEvaluationRatings { get; set; }
		public DbSet<User> Users { get; set; }
        public DbSet<ActivitiesSummary> ActivitiesSummaries { get; set; }
		public DbSet<OAS> OAS { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SummaryEvaluation> SummaryEvaluations { get; set; }
        public DbSet<TimekeepingSummary> TimekeepingSummaries { get; set; }
        public DbSet<BiometricLog> BiometricLogs { get; set; }
        public DbSet<Validation> Validations { get; set; }
        public DbSet<NASSchoolYearSemester> NASSchoolYears { get; set; }
        public DbSet<DailyTimeRecord> DailyTimeRecords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // set table names as singular
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Superior>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Superior>(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            /*modelBuilder.Entity<Office>()
                .HasOne(o => o.Superior)
                .WithOne()
                .HasForeignKey<Office>(o => o.SuperiorId)
                .OnDelete(DeleteBehavior.NoAction);*/

            /*modelBuilder.Entity<Office>()
                .HasMany(o => o.NAS)
                .WithOne(n => n.Office)
                .HasForeignKey(n => n.OfficeId)
                .OnDelete(DeleteBehavior.NoAction);*/


            modelBuilder.Entity<NAS>()
                .HasOne(n => n.User)
                .WithOne()
                .HasForeignKey<NAS>(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            /*modelBuilder.Entity<NAS>()
                .HasOne(n => n.Office)    
                .WithMany(o => o.NAS)  
                .HasForeignKey(n => n.OfficeId)
                .OnDelete(DeleteBehavior.NoAction);*/

            modelBuilder.Entity<NAS>()
                .HasMany(n => n.BiometricLogs)
                .WithOne(b => b.NAS)
                .HasForeignKey(b => b.NASId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<SuperiorEvaluationRating>()
                .HasOne(r => r.NAS)
                .WithMany()
                .HasForeignKey(n => n.NASId)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Office>().HasData(
            //new Office
            //{
            //    Id = 1,
            //    SuperiorId = 1,
            //});

            /*modelBuilder.Entity<NAS>()
                .HasMany(n => n.SchoolYears)
                .WithMany()
                .UsingEntity(j => j.ToTable("NAS_SchoolYears"));

            modelBuilder.Entity<NAS>()
                .HasMany(n => n.Semesters)
                .WithMany()
                .UsingEntity(j => j.ToTable("NAS_StudentSemesters"));*/
        }
    }
}
