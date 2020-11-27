using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RidePal.Data.Configurations;
using RidePal.Data.Seeder;
using RidePal.Models;
using System;
using System.Linq;

namespace RidePal.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<TrackPlaylist> TrackPlaylists { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            UserSeeder.SeedRoles(builder);

            builder.ApplyConfiguration(new ArtistConfig());
            builder.ApplyConfiguration(new GenreConfig());
            builder.ApplyConfiguration(new TrackConfig());
            builder.ApplyConfiguration(new PlaylistConfig());
            builder.ApplyConfiguration(new TrackPlaylistConfig());

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);


            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }


    }
}
