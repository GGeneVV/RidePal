using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Models;

namespace RidePal.Data.Configurations
{
    public class TrackPlaylistConfig : IEntityTypeConfiguration<TrackPlaylist>
    {
        public void Configure(EntityTypeBuilder<TrackPlaylist> trackPlaylist)
        {
            trackPlaylist
                .HasKey(trackPlaylist => new { trackPlaylist.TrackId, trackPlaylist.PlaylistId });

            trackPlaylist
                .HasOne(trackPlaylis => trackPlaylis.Playlist)
                .WithMany(trackPlaylist => trackPlaylist.TrackPlaylists)
                .HasForeignKey(trackPlaylist => trackPlaylist.PlaylistId);

            trackPlaylist
                .HasOne(trackPlaylist => trackPlaylist.Track)
                .WithMany(trackPlaylist => trackPlaylist.TrackPlaylists)
                .HasForeignKey(trackPlaylist => trackPlaylist.TrackId);
        }

    }
}
