using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Models;

namespace RidePal.Data.Configurations
{
    public class GenreConfig : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> genre)
        {
            genre
                .HasMany(g => g.Tracks)
                .WithOne(s => s.Genre)
                .HasForeignKey(s => s.GenreId);
        }

    }

}
