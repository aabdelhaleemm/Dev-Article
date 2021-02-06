using System;
using System.Reflection;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db
{
    public class ApplicationDbContext : IdentityDbContext<Users,Role,int>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
 //dotnet ef database update -s SchoolWebApi -p Infrastructur/
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Posts> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
          
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }
    }
}