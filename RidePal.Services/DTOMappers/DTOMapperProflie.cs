using AutoMapper;
using RidePal.Models;
using RidePal.Services.DTOModels;

namespace RidePal.Services.DTOMappers
{
    public class DTOMapperProflie : Profile
    {
        public DTOMapperProflie()
        {
            CreateMap<Album, AlbumDTO>().ReverseMap();

            CreateMap<Artist, ArtistDTO>().ReverseMap();

            CreateMap<Genre, GenreDTO>().ReverseMap();


            CreateMap<Playlist, PlaylistDTO>().ReverseMap();

            CreateMap<Track, TrackDTO>().ReverseMap();

            //CreateMap<Track, TrackDTO>()
            //    .ForPath(d => d.Genre, m => m.MapFrom(g => g.Genre))
            //    .ForPath(d => d.Artist, opt => opt.MapFrom(a => a.Artist))
            //    .ReverseMap();//.ForMember(g => g.Genre, opt => opt.MapFrom(s => s.Genre)).ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<TrackPlaylist, TrackPlaylistDTO>().ReverseMap();
        }
    }

}