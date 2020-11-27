using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Models;

namespace RidePal.Data.Configurations
{
    public class PlaylistConfig : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> playlist)
        {
            playlist
                .HasKey(track => track.Id);

            playlist
                .HasOne(playlist => playlist.User)
                .WithMany(user => user.Playlists)
                .HasForeignKey(playlist => playlist.UserId);

        }
    }

}
