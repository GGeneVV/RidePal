using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RidePal.Models;
using RidePal.Models.DataSource;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RidePal.Data.Seeder
{
    public static class Seeder
    {
        public static async Task SeedDbAsync(this AppDbContext _appDbContext)
        {
            var client = new HttpClient();

            var artists = new HashSet<Artist>();
            var artistDeezerId = new HashSet<string>();
            var albums = new HashSet<Album>();
            var albumDeezerId = new HashSet<string>();
            var trackHashset = new HashSet<Track>();
            var trackDeezerId = new HashSet<string>();

            using (var response = await client.GetAsync("https://api.deezer.com/genre"))
            {
                var responseAsString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DataGenre>(responseAsString);


                for (int i = 1; i < 11; i++) //result.Genres.Count()
                {
                    var genre = result.Genres[i];
                    await _appDbContext.Genres.AddAsync(genre);

                    //Thread.Sleep(300);
                    using (var responseSecond = await client.GetAsync($"https://api.deezer.com/search/playlist?q={genre.Name}&limit=50"))
                    {
                        var responseSecondAsString = await responseSecond.Content.ReadAsStringAsync();

                        var playlistTracks = JObject.Parse(responseSecondAsString)["data"];

                        var counter = 0;


                        for (int t = 0; t < playlistTracks.Count(); t++)
                        {
                            if (counter >= 1500)
                            {
                                break;
                            }

                            var tracklistURL = playlistTracks[t]["tracklist"].ToString();


                            using (var responseThird = await client.GetAsync($"{tracklistURL}&limit=100"))
                            {
                                var responseThirdAsString = await responseThird.Content.ReadAsStringAsync();

                                var tracks = JObject.Parse(responseThirdAsString)["data"]; //tracks as json
                                if (tracks == null)
                                {
                                    continue;
                                }

                                for (int j = 0; j < tracks.Count(); j++)
                                {
                                    if (counter >= 1500)
                                    {
                                        break;
                                    }

                                    if (tracks[j]["artist"] == null || tracks[j]["album"] == null || tracks[j] == null)
                                    {
                                        continue;
                                    }
                                    var track = JsonConvert.DeserializeObject<Track>(tracks[j].ToString());
                                    var trackArtist = JsonConvert.DeserializeObject<Artist>(tracks[j]["artist"].ToString());
                                    var trackAlbum = JsonConvert.DeserializeObject<Album>(tracks[j]["album"].ToString());
                                    if (artistDeezerId.Add(trackArtist.DeezerId))
                                    {
                                        artists.Add(trackArtist);
                                        track.Artist = trackArtist;
                                        trackAlbum.Artist = trackArtist;
                                    }
                                    else
                                    {
                                        var temp = artists.FirstOrDefault(a => a.DeezerId == trackArtist.DeezerId);
                                        track.Artist = temp;
                                        trackAlbum.Artist = temp;
                                    }

                                    if (albumDeezerId.Add(trackAlbum.DeezerId))
                                    {
                                        albums.Add(trackAlbum);
                                        track.Album = trackAlbum;
                                    }
                                    else
                                    {
                                        track.Album = albums.FirstOrDefault(a => a.DeezerId == trackAlbum.DeezerId);
                                    }

                                    track.Genre = genre;

                                    if (trackDeezerId.Add(track.DeezerId))
                                    {
                                        trackHashset.Add(track);
                                        counter++;
                                    }
                                    //await _appDbContext.SaveChangesAsync();
                                }
                                //Thread.Sleep(500);
                            }

                        }

                    }
                }
                await _appDbContext.Artists.AddRangeAsync(artists);
                await _appDbContext.Albums.AddRangeAsync(albums);
                await _appDbContext.Tracks.AddRangeAsync(trackHashset);

                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
