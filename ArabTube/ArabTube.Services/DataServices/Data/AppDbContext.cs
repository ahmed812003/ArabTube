using ArabTube.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) 
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().HasMany(ap => ap.Comments).WithOne(c => c.AppUser).HasForeignKey(c => c.UserId);
            builder.Entity<Video>().HasMany(v => v.Comments).WithOne(c => c.Video).HasForeignKey(c => c.VideoId);
            builder.Entity<Comment>().HasMany(c => c.Childrens).WithOne(c => c.ParentComment).HasForeignKey(c => c.ParentCommentId);


        }

        DbSet<Video> Videos { get; set; }

        DbSet<Comment> Comments { get; set; }

    }
}
