using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Chinook.Domain.Converters;


namespace Chinook.Domain.Entities
{
    public class InvoiceLine { 
        public int InvoiceLineId { get; set; }
        public int InvoiceId { get; set; }
        public int TrackId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public virtual Invoice Invoice { get; set; }
        [JsonIgnore]
        public virtual Track Track { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string TrackName { get; internal set; }
    }
}