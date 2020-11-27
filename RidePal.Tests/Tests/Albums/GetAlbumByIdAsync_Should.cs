using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ridepal.Tests;
using RidePal.Data;
using RidePal.Models;
using RidePal.Services;
using RidePal.Services.DTOModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class GetAlbumByIdAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrectAlbum()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrectAlbum));

            using (var arrangeContext = new AppDbContext(options))
            {
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("Name", 777));

                await arrangeContext.SaveChangesAsync();

                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "Name",
                    DeezerId = "DeezerId",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id
                });

                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedAlbum = await actContext.Albums.FirstAsync();

                var expectedAlbumDTO = Utils.Mapper.Map<AlbumDTO>(expectedAlbum);
                var sut = new AlbumService(actContext, Utils.Mapper);

                //Act
                var actualAlbum = await sut.GetAlbumByIdAsync(expectedAlbum.Id);

                //Assert
                Assert.AreEqual(expectedAlbumDTO.Id, actualAlbum.Id);
                Assert.AreEqual(expectedAlbumDTO.Title, actualAlbum.Title);
                Assert.AreEqual(expectedAlbumDTO.Tracklist, actualAlbum.Tracklist);
            }
        }
        [TestMethod]
        public async Task Throw_When_AlbumNotFound()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(Throw_When_AlbumNotFound)); ;

            var context = new AppDbContext(options);
            var sut = new AlbumService(context, Utils.Mapper);

            //Act & Assert
            Assert.IsNull(await sut.GetAlbumByIdAsync(Guid.NewGuid()));
        }
    }
}
