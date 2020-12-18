using Chinook.Domain.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Chinook.Domain.Entities
{
    public class Album  
    {
        public Album()
        {
            Tracks = new HashSet<Track>();
        }

        [Key]
        public int AlbumId { get; set; }
        
        [StringLength(160, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        public int ArtistId { get; set; }

        [JsonIgnore]
        public virtual Artist Artist { get; set; }
        [JsonIgnore]
        public virtual ICollection<Track> Tracks { get; set; }

        [NotMapped]
        public string ArtistName { get; set; }
    }
}