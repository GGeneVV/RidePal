using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Models;
using RidePal.Services;
using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ridepal.Tests.ArtistTests
{
    [TestClass]
    public class GetAllArtistsAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrect_ArtistCollection()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_ArtistCollection));

            using (var arrangeContext = new AppDbContext(options))
            {
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("ANameFirst", 700));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("BNameSecond", 701));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("CNameThird", 703));
                await arrangeContext.SaveChangesAsync();

                await arrangeContext.Albums.AddAsync(Utils.CreateMockAlbum("AAlbumTitle", 1,
                    arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "700").Id));
                await arrangeContext.Albums.AddAsync(Utils.CreateMockAlbum("BAlbumTitle", 2,
                    arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "701").Id));
                await arrangeContext.Albums.AddAsync(Utils.CreateMockAlbum("CAlbumTitle", 3,
                    arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "703").Id));
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedArtists = actContext.Artists
                    .Select(x => Utils.Mapper.Map<ArtistDTO>(x))
                    .ToList();

                var sut = new ArtistService(actContext, Utils.Mapper);

                //Act
                var actualArtists = sut.GetAllArtists().ToList();

                //Assert
                Assert.AreEqual(expectedArtists.Count, actualArtists.Count);
                Assert.AreEqual(expectedArtists.ElementAt(1).Name, actualArtists.ElementAt(1).Name);
                Assert.AreEqual(expectedArtists.ElementAt(2).Id, actualArtists.ElementAt(2).Id);

            }
        }

        [TestMethod]
        public async Task ReturnCorrect_Artists_With_DefaultSortOrder()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_Artists_With_DefaultSortOrder));

            using (var arrangeContext = new AppDbContext(options))
            {
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameFirst", 700));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameSecond", 701));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameThird", 703));
                await arrangeContext.SaveChangesAsync();

                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "NameOfFirst",
                    DeezerId = "775",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault(x=>x.DeezerId=="700").Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "NameOfSecond",
                    DeezerId = "776",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault(x => x.DeezerId == "700").Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "NameOfThird",
                    DeezerId = "777",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault(x => x.DeezerId == "700").Id

                });
                await arrangeContext.SaveChangesAsync();

            }

            using (var actContext = new AppDbContext(options))
            {

                var artists = actContext.Artists.Select(artist => new ArtistDTO()
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    Picture = artist.Picture,
                    Albums = artist.Albums.Select(x => Utils.Mapper.Map<AlbumDTO>(x)).ToList(),
                }).ToList();

                var expectedArtists = artists
                    .OrderBy(a=>a.Albums.Count())
                    .ToList();

                var sut = new ArtistService(actContext, Utils.Mapper);

                //Act
                var actualArtists = sut.GetAllArtists(sortOrder: "default").ToList();

                //Assert
                Assert.AreEqual(expectedArtists.ElementAt(0).Albums.Count(), actualArtists.ElementAt(0).Albums.Count());
                Assert.AreEqual(expectedArtists.ElementAt(0).Name, actualArtists.ElementAt(0).Name);
                Assert.AreEqual(expectedArtists.ElementAt(0).Id, actualArtists.ElementAt(0).Id);

                Assert.AreEqual(expectedArtists.ElementAt(1).Albums.Count(), actualArtists.ElementAt(1).Albums.Count());
                Assert.AreEqual(expectedArtists.ElementAt(1).Name, actualArtists.ElementAt(1).Name);
                Assert.AreEqual(expectedArtists.ElementAt(1).Id, actualArtists.ElementAt(1).Id);

                Assert.AreEqual(expectedArtists.ElementAt(2).Albums.Count(), actualArtists.ElementAt(2).Albums.Count());
                Assert.AreEqual(expectedArtists.ElementAt(2).Name, actualArtists.ElementAt(2).Name);
                Assert.AreEqual(expectedArtists.ElementAt(2).Id, actualArtists.ElementAt(2).Id);

            }
        }

        [TestMethod]
        public async Task ReturnCorrect_Albums_Ordered_ByAlbumCountDec_Correctly()
        {
            var options = Utils.GetOptions(nameof(ReturnCorrect_Albums_Ordered_ByAlbumCountDec_Correctly));

            using (var arrangeContext = new AppDbContext(options))
            {
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("TheOneWeWant", 700));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("BNameSecond", 701));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("CNameThird", 703));
                await arrangeContext.SaveChangesAsync();


               
                await arrangeContext.Albums.AddAsync(Utils.CreateMockAlbum("AAlbumTitle", 1,
                    arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "700").Id));
               var albumTwo = await arrangeContext.Albums.AddAsync(Utils.CreateMockAlbum("BAlbumTitle", 2,
                    arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "701").Id));
               var AlbumThree = await arrangeContext.Albums.AddAsync(Utils.CreateMockAlbum("CAlbumTitle", 3,
                    arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "703").Id));
                await arrangeContext.SaveChangesAsync();

               
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedArtists = actContext.Artists
                    .Where(n => n.Name.Contains("TheOneWeWant"))
                    .Select(x => Utils.Mapper.Map<ArtistDTO>(x))
                    .ToList();

                var sut = new ArtistService(actContext, Utils.Mapper);

                //Act
                var actualArtists = sut.GetAllArtists(searchString: "TheOneWeWant").ToList();

                //Assert
                Assert.AreEqual(expectedArtists.Count, actualArtists.Count);
                Assert.AreEqual(expectedArtists.ElementAt(0).Name, actualArtists.ElementAt(0).Name);
                Assert.AreEqual(expectedArtists.ElementAt(0).Id, actualArtists.ElementAt(0).Id);

            }
        }
    }
}
