using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Doman.Entities;

namespace BE_API_BASE.Infrastructure.DataContexts
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
      
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RefeshToken> RefeshTokens { get; set; }
        public virtual DbSet<ConfirmEmail> ConfirmEmails { get; set; }
        public async Task<int> CommitChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public DbSet<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           SeedRoles(modelBuilder);
        }*/

        /*private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData
                (
                    new Role {Id = 1, RoleCode = "Admin" , RoleName = "Admin", IsActive=false},
                    new Role {Id = 2, RoleCode = "User" , RoleName = "User", IsActive = false }
                );
        }*/
    }
}
