using Microsoft.EntityFrameworkCore;
using PageDataRepositories.Models;
using System;

namespace PageDataRepositories.Repositories.DataSources.Contexts
{
    public class SqlDataContext : DbContext
    {
        public SqlDataContext(DbContextOptions<SqlDataContext> options) : base(options) { }

        public DbSet<PageBehaviour> PageBehaviours { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PageBehaviour>(entity =>
            {
                entity.ToTable("PageBehaviours");
                entity.HasKey(e => e.BehaviourId);

                entity.Property(e => e.Browser).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Ip).HasMaxLength(500).IsRequired();
                entity.Property(e => e.PageName).HasMaxLength(500).IsRequired();
                entity.Property(e => e.PageParams).IsRequired();
            });
        }
    }
}
