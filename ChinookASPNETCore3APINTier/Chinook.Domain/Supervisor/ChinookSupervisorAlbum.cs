using System;
using System.Collections.Generic;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public IEnumerable<Album> GetAllAlbum()
        {
            var albums = _albumRepository.GetAll();
            foreach (var album in albums)
            {
                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Album-", album.AlbumId), album, cacheEntryOptions);
            }

            return albums;
        }

        public Album GetAlbumById(int id)
        {
            var albumCached = _cache.Get<Album>(string.Concat("Album-", id));

            if (albumCached != null)
            {
                return albumCached;
            }
            else
            {
                var album = _albumRepository.GetById(id);
                if (album == null) return null;
                album.ArtistName = (_artistRepository.GetById(album.ArtistId)).Name;

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Album-", album.AlbumId), album, cacheEntryOptions);

                return album;
            }
        }

        public IEnumerable<Album> GetAlbumByArtistId(int id)
        {
            var albums = _albumRepository.GetByArtistId(id);
            return albums;
        }

        public Album AddAlbum(Album newAlbum)
        {
            var album = newAlbum;

            album = _albumRepository.Add(album);
            newAlbum.AlbumId = album.AlbumId;
            return newAlbum;
        }

        public bool UpdateAlbum(Album _album)
        {
            var album = _albumRepository.GetById(_album.AlbumId);

            if (album is null) return false;
            album.AlbumId = album.AlbumId;
            album.Title = album.Title;
            album.ArtistId = album.ArtistId;

            return _albumRepository.Update(album);
        }

        public bool DeleteAlbum(int id)
            => _albumRepository.Delete(id);
    }
}