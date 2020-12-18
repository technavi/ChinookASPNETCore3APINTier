using System;
using System.Collections.Generic;
using Chinook.Domain.Extensions;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Chinook.Domain.Entities;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public IEnumerable<Playlist> GetAllPlaylist()
        {
            var playlists = _playlistRepository.GetAll();
            foreach (var playlist in playlists)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Playlist-", playlist.PlaylistId), playlist, cacheEntryOptions);
            }
            return playlists;
        }

        public Playlist GetPlaylistById(int id)
        {
            var playlistCached = _cache.Get<Playlist>(string.Concat("Playlist-", id));

            if (playlistCached != null)
            {
                return playlistCached;
            }
            else
            {
                var playlist = (_playlistRepository.GetById(id));
                playlist.Tracks = (GetTrackByPlaylistId(playlist.PlaylistId)).ToList();

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Playlist-", playlist.PlaylistId), playlist, cacheEntryOptions);

                return playlist;
            }
        }

        public Playlist AddPlaylist(Playlist newPlaylist)
        {
            var playlist = newPlaylist;

            playlist = _playlistRepository.Add(playlist);
            newPlaylist.PlaylistId = playlist.PlaylistId;
            return newPlaylist;
        }

        public bool UpdatePlaylist(Playlist _playlist)
        {
            var playlist = _playlistRepository.GetById(_playlist.PlaylistId);

            if (playlist == null) return false;
            playlist.PlaylistId = playlist.PlaylistId;
            playlist.Name = playlist.Name;

            return _playlistRepository.Update(playlist);
        }

        public bool DeletePlaylist(int id) 
            => _playlistRepository.Delete(id);
        
        public IEnumerable<Playlist> GetPlaylistByTrackId(int id)
        {
            var playlists = _playlistRepository.GetByTrackId(id);
            return playlists;
        }
    }
}