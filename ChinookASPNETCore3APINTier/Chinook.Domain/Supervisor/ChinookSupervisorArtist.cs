using System;
using System.Collections.Generic;
using System.Linq;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;

using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public IEnumerable<Artist> GetAllArtist()
        {
            var artists = _artistRepository.GetAll();
            foreach (var artist in artists)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Artist-", artist.ArtistId), artist, cacheEntryOptions);
            }
            return artists;
        }

        public Artist GetArtistById(int id)
        {
            var artistCached = _cache.Get<Artist>(string.Concat("Artist-", id));

            if (artistCached != null)
            {
                return artistCached;
            }
            else
            {
                var artist = (_artistRepository.GetById(id));
                artist.Albums = (GetAlbumByArtistId(artist.ArtistId)).ToList();

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Artist-", artist.ArtistId), artist, cacheEntryOptions);

                return artist;
            }
        }

        public Artist AddArtist(Artist newArtist)
        {
            var artist = newArtist;

            artist = _artistRepository.Add(artist);
            newArtist.ArtistId = artist.ArtistId;
            return newArtist;
        }

        public bool UpdateArtist(Artist _artist)
        {
            var artist = _artistRepository.GetById(_artist.ArtistId);

            if (artist == null) return false;
            artist.ArtistId = artist.ArtistId;
            artist.Name = artist.Name;

            return _artistRepository.Update(artist);
        }

        public bool DeleteArtist(int id) 
            => _artistRepository.Delete(id);
    }
}