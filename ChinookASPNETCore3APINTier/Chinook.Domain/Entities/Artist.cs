using Chinook.Domain.Converters;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chinook.Domain.Entities
{
    public class Artist  
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
        }

        public int ArtistId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Album> Albums { get; set; }
         
    }
}