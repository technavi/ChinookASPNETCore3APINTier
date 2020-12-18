using System;
using System.Collections.Generic;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;

using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public IEnumerable<Track> GetAllTrack()
        {
            var tracks = _trackRepository.GetAll();
            foreach (var track in tracks)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Track-", track.TrackId), track, cacheEntryOptions);
            }
            return tracks;
        }

        public Track GetTrackById(int id)
        {
            var trackCached = _cache.Get<Track>(string.Concat("Track-", id));

            if (trackCached != null)
            {
                return trackCached;
            }
            else
            {
                var track = (_trackRepository.GetById(id));
                track.Genre = GetGenreById(track.GenreId.GetValueOrDefault());
                track.Album = GetAlbumById(track.AlbumId);
                track.MediaType = GetMediaTypeById(track.MediaTypeId);
                if (track.Album != null)
                {
                    track.AlbumName = track.Album.Title;
                }
                track.MediaTypeName = track.MediaType.Name;
                if (track.Genre != null)
                {
                    track.GenreName = track.Genre.Name;   
                }

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Track-", track.TrackId), track, cacheEntryOptions);

                return track;
            }
        }

        public IEnumerable<Track> GetTrackByAlbumId(int id)
        {
            var tracks = _trackRepository.GetByAlbumId(id);
            return tracks;
        }

        public IEnumerable<Track> GetTrackByGenreId(int id)
        {
            var tracks = _trackRepository.GetByGenreId(id);
            return tracks;
        }

        public IEnumerable<Track> GetTrackByMediaTypeId(int id)
        {
            var tracks = _trackRepository.GetByMediaTypeId(id);
            return tracks;
        }

        public IEnumerable<Track> GetTrackByPlaylistId(int id)
        {
            var tracks = _trackRepository.GetByPlaylistId(id);
            return tracks;
        }

        public Track AddTrack(Track newTrack)
        {
            var track = newTrack;

            _trackRepository.Add(track);
            newTrack.TrackId = track.TrackId;
            return newTrack;
        }

        public bool UpdateTrack(Track _track)
        {
            var track = _trackRepository.GetById(_track.TrackId);

            if (track == null) return false;
            track.TrackId = track.TrackId;
            track.Name = track.Name;
            track.AlbumId = track.AlbumId;
            track.MediaTypeId = track.MediaTypeId;
            track.GenreId = track.GenreId;
            track.Composer = track.Composer;
            track.Milliseconds = track.Milliseconds;
            track.Bytes = track.Bytes;
            track.UnitPrice = track.UnitPrice;

            return _trackRepository.Update(track);
        }

        public bool DeleteTrack(int id) 
            => _trackRepository.Delete(id);
        
        public IEnumerable<Track> GetTrackByArtistId(int id)
        {
            var tracks = _trackRepository.GetByArtistId(id);
            return tracks;
        }

        public IEnumerable<Track> GetTrackByInvoiceId(int id)
        {
            var tracks = _trackRepository.GetByInvoiceId(id);
            return tracks;
        }
    }
}