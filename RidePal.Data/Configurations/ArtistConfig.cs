using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Models;

namespace RidePal.Data.Configurations
{
    public class ArtistConfig : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> artist)
        {
            artist
                .HasMany(artist => artist.Albums)
                .WithOne(album => album.Artist)
                .HasForeignKey(album => album.ArtistId);

            artist
                .HasMany(a => a.Tracks)
                .WithOne(s => s.Artist)
                .HasForeignKey(s => s.ArtistId);
        }
    }

}
