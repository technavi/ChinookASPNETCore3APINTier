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
        public IEnumerable<Genre> GetAllGenre()
        {
            var genres = _genreRepository.GetAll();

            foreach (var genre in genres)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Genre-", genre.GenreId), genre, cacheEntryOptions);
            }
            
            return genres;
        }

        public Genre GetGenreById(int id)
        {
            var genreCached = _cache.Get<Genre>(string.Concat("Genre-", id));

            if (genreCached != null)
            {
                return genreCached;
            }
            else
            {
                var genre = _genreRepository.GetById(id);
                if (genre != null) return null;
                genre.Tracks = (GetTrackByGenreId(genre.GenreId)).ToList();
                
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Genre-", genre.GenreId), genre, cacheEntryOptions);
                
                return genre;
            }
        }

        public Genre AddGenre(Genre newGenre)
        {
            var genre = newGenre;

            genre = _genreRepository.Add(genre);
            newGenre.GenreId = genre.GenreId;
            return newGenre;
        }

        public bool UpdateGenre(Genre _genre)
        {
            var genre = _genreRepository.GetById(_genre.GenreId);

            if (genre == null) return false;
            genre.GenreId = genre.GenreId;
            genre.Name = genre.Name;

            return _genreRepository.Update(genre);
        }

        public bool DeleteGenre(int id) 
            => _genreRepository.Delete(id);
    }
}