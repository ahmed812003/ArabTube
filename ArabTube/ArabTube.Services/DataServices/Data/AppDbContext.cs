using ArabTube.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

            builder.Entity<AppUser>()
                .HasMany(ap => ap.Videos)
                .WithOne(c => c.AppUser)
                .HasForeignKey(c => c.UserId);


            builder.Entity<AppUser>()
                .HasMany(ap => ap.WatchedVideos)
                .WithMany(v => v.Viewers)
                .UsingEntity<WatchedVideo>();

            builder.Entity<WatchedVideo>()
                .HasOne(wv => wv.User)
                .WithMany(ap => ap.History);

            builder.Entity<WatchedVideo>()
                .HasOne(wv => wv.Video)
                .WithMany(v => v.History);

            builder.Entity<AppUserConnection>()
            .HasKey(auc => new { auc.FollowerId, auc.FollowingId });

            builder.Entity<AppUserConnection>()
                .HasOne(auc => auc.Follower)
                .WithMany(au => au.Following)
                .HasForeignKey(auc => auc.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUserConnection>()
                .HasOne(auc => auc.Following)
                .WithMany(au => au.Followers)
                .HasForeignKey(auc => auc.FollowingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasMany(ap => ap.Playlists)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.Entity<Video>()
            .HasMany(v => v.Playlists)
            .WithMany(pl => pl.Videos)
            .UsingEntity<PlaylistVideo>();

            builder.Entity<PlaylistVideo>()
                .HasOne(plv => plv.Video)
                .WithMany(v => v.PlaylistVideos);
            
            builder.Entity<PlaylistVideo>()
                .HasOne(plv => plv.Playlist)
                .WithMany(v => v.PlaylistVideos);

            builder.Entity<AppUser>()
                .HasMany(ap => ap.Comments)
                .WithOne(c => c.AppUser)
                .HasForeignKey(c => c.UserId);

            builder.Entity<Video>()
                .HasMany(v => v.Comments)
                .WithOne(c => c.Video)
                .HasForeignKey(c => c.VideoId);

            builder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Childrens)
            .HasForeignKey(c => c.ParentCommentId);


            builder.Entity<Video>()
            .HasMany(v => v.UsersLikedVideo)
            .WithMany(u => u.LikedVideos)
            .UsingEntity<VideoLike>();

            builder.Entity<VideoLike>()
                .HasOne(vl => vl.AppUser)
                .WithMany(u => u.VideosLikes);

            builder.Entity<VideoLike>()
                .HasOne(vl => vl.Video)
                .WithMany(v => v.VideosLikes);


            builder.Entity<Video>()
            .HasMany(v => v.UsersDislikedVideo)
            .WithMany(u => u.DislikedVideos)
            .UsingEntity<VideoDislike>();

            builder.Entity<VideoDislike>()
                .HasOne(vdl => vdl.User)
                .WithMany(u => u.VideosDislikes);

            builder.Entity<VideoDislike>()
                .HasOne(vdl => vdl.Video)
                .WithMany(v => v.VideosDislikes);


            builder.Entity<Video>()
            .HasMany(v => v.UsersFlagedVideo)
            .WithMany(u => u.FlagedVideos)
            .UsingEntity<VideoFlag>();

            builder.Entity<VideoFlag>()
                .HasOne(vf => vf.User)
                .WithMany(u => u.VideosFlags);

            builder.Entity<VideoFlag>()
                .HasOne(vf => vf.Video)
                .WithMany(v => v.VideosFlags);

        }


        public DbSet<Video> Videos { get; set; }
        public DbSet<AppUserConnection> Subscribers { get; set; }
        public DbSet<WatchedVideo> WatchedVideos { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistVideo> PlaylistVideos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<VideoLike> VideosLikes { get; set; }
        public DbSet<VideoDislike> VideosDislikes { get; set; }
        public DbSet<VideoFlag> VideosFlags { get; set; }
    }
}
