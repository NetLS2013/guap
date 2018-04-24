using Newtonsoft.Json;

namespace Guap.Models
{
    public class Fiat
    {
        [JsonProperty("price_usd")]
        public string PriceUsd { get; set; }
    }
}