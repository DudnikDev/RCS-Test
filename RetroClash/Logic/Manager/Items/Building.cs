using Newtonsoft.Json;

namespace RetroClash.Logic.Manager.Items
{
    public class Building
    { 
        [JsonProperty("attack_mode")]
        public bool AttackMode { get; set; }

        [JsonProperty("data")]
        public int Data { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lvl",  DefaultValueHandling = DefaultValueHandling.Include)]
        public int Level { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("res_time")]
        public int ResetTime { get; set; }

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("unit_prod")]
        public UnitProd UnitProd { get; set; }

        [JsonProperty("storage_type")]
        public int StorageType { get; set; }
    }
}