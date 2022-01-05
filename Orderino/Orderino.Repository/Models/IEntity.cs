using Newtonsoft.Json;

namespace Orderino.Repository.Models
{
    public interface IEntity
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }
    }
}
