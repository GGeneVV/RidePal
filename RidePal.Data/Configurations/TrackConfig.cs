using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Models;

namespace RidePal.Data.Configurations
{
    public class TrackConfig : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> track)
        {
            track
                .HasOne(s => s.Album)
                .WithMany(a => a.Tracks)
                .HasForeignKey(s => s.AlbumId);

            track
                .HasOne(s => s.Artist)
                .WithMany(a => a.Tracks)
                .HasForeignKey(s => s.ArtistId);

            track
                .HasOne(s => s.Genre)
                .WithMany(g => g.Tracks)
                .HasForeignKey(s => s.GenreId);
        }
    }

}
