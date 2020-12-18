using Chinook.Domain.Converters;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Chinook.Domain.Entities
{
    public class Playlist  
    {
        public Playlist()
        {
            PlaylistTracks = new HashSet<PlaylistTrack>();
        }

        public int PlaylistId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<Track> Tracks { get; internal set; }
    }
}