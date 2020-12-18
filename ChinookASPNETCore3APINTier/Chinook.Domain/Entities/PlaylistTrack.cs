using System.Text.Json.Serialization;
using Chinook.Domain.Converters;


namespace Chinook.Domain.Entities
{
    public class PlaylistTrack
    {
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }

        [JsonIgnore]
        public virtual Playlist Playlist { get; set; }
        [JsonIgnore]
        public virtual Track Track { get; set; }


    }
}