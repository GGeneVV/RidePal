using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data;
using RidePal.Models;
using RidePal.Services;
using RidePal.Services.DTOModels;
using System.Linq;
using System.Threading.Tasks;

namespace Ridepal.Tests.AlbumTests
{

    [TestClass]
    public class GetAllAbumsAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrect_AlbumCollection()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_AlbumCollection));

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
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "NameOfSecond",
                    DeezerId = "776",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "NameOfThird",
                    DeezerId = "777",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedAlbums = actContext.Albums
                    .Select(x => Utils.Mapper.Map<AlbumDTO>(x))
                    .ToList();

                var sut = new AlbumService(actContext, Utils.Mapper);

                //Act
                var actualAlbums = sut.GetAllAlbums().ToList();

                //Assert
                Assert.AreEqual(expectedAlbums.Count, actualAlbums.Count);
                Assert.AreEqual(expectedAlbums.ElementAt(1).Picture, actualAlbums.ElementAt(1).Picture);
                Assert.AreEqual(expectedAlbums.ElementAt(2).ArtistId, actualAlbums.ElementAt(2).ArtistId);

            }
        }

        [TestMethod]
        public async Task ReturnCorrect_Albums_With_SearchParam()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_Albums_With_SearchParam));

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
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "NameOfSecond",
                    DeezerId = "776",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "NameOfThird",
                    DeezerId = "777",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedAlbums = actContext.Albums
                    .Where(x => x.Title.Contains("Third"))
                    .Select(x => Utils.Mapper.Map<AlbumDTO>(x))
                    .ToList();

                var sut = new AlbumService(actContext, Utils.Mapper);

                //Act
                var actualAlbums = sut.GetAllAlbums(searchString: "Third").ToList();

                //Assert
                Assert.AreEqual(expectedAlbums.Count, actualAlbums.Count);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Title, actualAlbums.ElementAt(0).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Id, actualAlbums.ElementAt(0).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(0).ArtistId, actualAlbums.ElementAt(0).ArtistId);
            }
        }

        [TestMethod]
        public async Task ReturnCorrect_Albums_Ordered_ByTitle_Desc_Correctly()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_Albums_Ordered_ByTitle_Desc_Correctly));

            using (var arrangeContext = new AppDbContext(options))
            {

                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameFirst", 700));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameSecond", 701));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameThird", 703));
                await arrangeContext.SaveChangesAsync();

                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "ANameOfFirst",
                    DeezerId = "775",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "BNameOfSecond",
                    DeezerId = "776",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "CNameOfThird",
                    DeezerId = "777",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedAlbums = actContext.Albums
                    .OrderByDescending(x => x.Title)
                    .Select(x => Utils.Mapper.Map<AlbumDTO>(x))
                    .ToList();

                var sut = new AlbumService(actContext, Utils.Mapper);

                //Act
                var actualAlbums = sut.GetAllAlbums(sortOrder: "title_desc").ToList();

                //Assert
                Assert.AreEqual(expectedAlbums.Count, actualAlbums.Count);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Title, actualAlbums.ElementAt(0).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Id, actualAlbums.ElementAt(0).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(0).ArtistId, actualAlbums.ElementAt(0).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(1).Title, actualAlbums.ElementAt(1).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(1).Id, actualAlbums.ElementAt(1).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(1).ArtistId, actualAlbums.ElementAt(1).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(2).Title, actualAlbums.ElementAt(2).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(2).Id, actualAlbums.ElementAt(2).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(2).ArtistId, actualAlbums.ElementAt(2).ArtistId);
            }
        }

        [TestMethod]
        public async Task ReturnCorrect_Albums_Ordered_ByTitle_Correctly()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_Albums_Ordered_ByTitle_Correctly));

            using (var arrangeContext = new AppDbContext(options))
            {

                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameFirst", 700));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameSecond", 701));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameThird", 703));
                await arrangeContext.SaveChangesAsync();

                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "ANameOfFirst",
                    DeezerId = "775",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "BNameOfSecond",
                    DeezerId = "776",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "CNameOfThird",
                    DeezerId = "777",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedAlbums = actContext.Albums
                    .OrderBy(x => x.Title)
                    .Select(x => Utils.Mapper.Map<AlbumDTO>(x))
                    .ToList();

                var sut = new AlbumService(actContext, Utils.Mapper);

                //Act
                var actualAlbums = sut.GetAllAlbums(sortOrder: "title").ToList();

                //Assert
                Assert.AreEqual(expectedAlbums.Count, actualAlbums.Count);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Title, actualAlbums.ElementAt(0).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Id, actualAlbums.ElementAt(0).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(0).ArtistId, actualAlbums.ElementAt(0).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(1).Title, actualAlbums.ElementAt(1).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(1).Id, actualAlbums.ElementAt(1).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(1).ArtistId, actualAlbums.ElementAt(1).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(2).Title, actualAlbums.ElementAt(2).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(2).Id, actualAlbums.ElementAt(2).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(2).ArtistId, actualAlbums.ElementAt(2).ArtistId);
            }
        }

        [TestMethod]
        public async Task ReturnCorrect_Albums_Ordered_ByArtistNameDesc_Correctly()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_Albums_Ordered_ByArtistNameDesc_Correctly));

            using (var arrangeContext = new AppDbContext(options))
            {

                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("ANameFirst", 700));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("BNameSecond", 701));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("CNameThird", 702));
                await arrangeContext.SaveChangesAsync();

                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "ANameOfFirst",
                    DeezerId = "775",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "700").Id,
                    Artist = arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "700")


                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "BNameOfSecond",
                    DeezerId = "776",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "701").Id,
                    Artist = arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "701")
                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "CNameOfThird",
                    DeezerId = "777",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "702").Id,
                    Artist = arrangeContext.Artists.FirstOrDefault(i => i.DeezerId == "702")

                });
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedAlbums = actContext.Albums
                    .OrderBy(x => x.Artist.Name)
                    .Select(x => Utils.Mapper.Map<AlbumDTO>(x))
                    .ToList();

                var sut = new AlbumService(actContext, Utils.Mapper);

                //Act
                var actualAlbums = sut.GetAllAlbums(sortOrder: "NameOfArtist_decs").ToList();

                //Assert
                Assert.AreEqual(expectedAlbums.Count, actualAlbums.Count);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Title, actualAlbums.ElementAt(0).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Id, actualAlbums.ElementAt(0).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(0).ArtistId, actualAlbums.ElementAt(0).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(1).Title, actualAlbums.ElementAt(1).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(1).Id, actualAlbums.ElementAt(1).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(1).ArtistId, actualAlbums.ElementAt(1).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(2).Title, actualAlbums.ElementAt(2).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(2).Id, actualAlbums.ElementAt(2).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(2).ArtistId, actualAlbums.ElementAt(2).ArtistId);
            }
        }

        [TestMethod]
        public async Task ReturnCorrect_Albums_Ordered_ByArtistName_Correctly()
        {
            //Arrange 
            var options = Utils.GetOptions(nameof(ReturnCorrect_Albums_Ordered_ByArtistName_Correctly));

            using (var arrangeContext = new AppDbContext(options))
            {

                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameFirst", 700));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameSecond", 701));
                await arrangeContext.Artists.AddAsync(Utils.CreateMockArtist("NameThird", 703));
                await arrangeContext.SaveChangesAsync();

                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "ANameOfFirst",
                    DeezerId = "775",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "BNameOfSecond",
                    DeezerId = "776",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.Albums.AddAsync(new Album
                {
                    Title = "CNameOfThird",
                    DeezerId = "777",
                    Tracklist = "SongListUrl",
                    ArtistId = arrangeContext.Artists.FirstOrDefault().Id

                });
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var expectedAlbums = actContext.Albums
                    .OrderBy(x => x.Artist.Name)
                    .Select(x => Utils.Mapper.Map<AlbumDTO>(x))
                    .ToList();

                var sut = new AlbumService(actContext, Utils.Mapper);

                //Act
                var actualAlbums = sut.GetAllAlbums(sortOrder: "NameOfArtist").ToList();

                //Assert
                Assert.AreEqual(expectedAlbums.Count, actualAlbums.Count);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Title, actualAlbums.ElementAt(0).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(0).Id, actualAlbums.ElementAt(0).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(0).ArtistId, actualAlbums.ElementAt(0).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(1).Title, actualAlbums.ElementAt(1).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(1).Id, actualAlbums.ElementAt(1).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(1).ArtistId, actualAlbums.ElementAt(1).ArtistId);

                Assert.AreEqual(expectedAlbums.ElementAt(2).Title, actualAlbums.ElementAt(2).Title);
                Assert.AreEqual(expectedAlbums.ElementAt(2).Id, actualAlbums.ElementAt(2).Id);
                Assert.AreEqual(expectedAlbums.ElementAt(2).ArtistId, actualAlbums.ElementAt(2).ArtistId);
            }
        }
    }
}
