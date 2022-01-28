using Newtonsoft.Json;

namespace Orderino.Server.Cards.Utils
{
    public class CardValue<T>
    {
        [JsonProperty("type")]
        public object Type { get; set; } = "task/fetch";

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
