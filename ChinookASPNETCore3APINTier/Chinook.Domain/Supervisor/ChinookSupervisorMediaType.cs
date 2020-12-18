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
        public IEnumerable<MediaType> GetAllMediaType()
        {
            var mediaTypes = _mediaTypeRepository.GetAll();
            foreach (var mediaType in mediaTypes)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("MediaType-", mediaType.MediaTypeId), mediaType, cacheEntryOptions);
            }
            return mediaTypes;
        }

        public MediaType GetMediaTypeById(int id)
        {
            var mediaTypeCached = _cache.Get<MediaType>(string.Concat("MediaType-", id));

            if (mediaTypeCached != null)
            {
                return mediaTypeCached;
            }
            else
            {
                var mediaType = (_mediaTypeRepository.GetById(id));
                mediaType.Tracks = (GetTrackByMediaTypeId(mediaType.MediaTypeId)).ToList();

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("MediaType-", mediaType.MediaTypeId), mediaType, cacheEntryOptions);

                return mediaType;
            }
        }

        public MediaType AddMediaType(MediaType newMediaType)
        {
            /*var mediaType = new MediaType
            {
                Name = newMediaType.Name
            };*/

            var mediaType = newMediaType;

            mediaType = _mediaTypeRepository.Add(mediaType);
            newMediaType.MediaTypeId = mediaType.MediaTypeId;
            return newMediaType;
        }

        public bool UpdateMediaType(MediaType _mediaType)
        {
            var mediaType = _mediaTypeRepository.GetById(_mediaType.MediaTypeId);

            if (mediaType == null) return false;
            mediaType.MediaTypeId = mediaType.MediaTypeId;
            mediaType.Name = mediaType.Name;

            return _mediaTypeRepository.Update(mediaType);
        }

        public bool DeleteMediaType(int id) 
            => _mediaTypeRepository.Delete(id);
    }
}