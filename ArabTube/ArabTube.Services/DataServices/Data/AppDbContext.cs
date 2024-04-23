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

            // comment table relationships
            builder.Entity<AppUser>().HasMany(ap => ap.Comments).WithOne(c => c.AppUser).HasForeignKey(c => c.UserId);
            builder.Entity<Video>().HasMany(v => v.Comments).WithOne(c => c.Video).HasForeignKey(c => c.VideoId);
            builder.Entity<Comment>().HasMany(c => c.Childrens).WithOne(c => c.ParentComment).HasForeignKey(c => c.ParentCommentId);


            // video table relationships


            // user table relationships 


            // playlist table relationships


            // fav playlist table relationships




        }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Comment> Comments { get; set; }
        
        /*public DbSet<FavPlaylist> FavPlaylists { get; set; }

        public DbSet<Playlist> Playlists { get; set; }*/

    }
}
