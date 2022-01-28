using Newtonsoft.Json;

namespace Orderino.Shared.Models
{
    public interface IEntity
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }
    }
}
