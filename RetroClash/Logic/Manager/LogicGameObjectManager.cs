using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RetroClash.Logic.Manager.Items;

namespace RetroClash.Logic.Manager
{
    public class LogicGameObjectManager
    {
        [JsonProperty("buildings")] public List<Building> Buildings = new List<Building>();

        [JsonProperty("decos")] public List<Decoration> Decorations = new List<Decoration>();

        [JsonProperty("obstacles")] public List<Obstacle> Obstacles = new List<Obstacle>();

        [JsonIgnore] public JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        [JsonProperty("traps")] public List<Trap> Traps = new List<Trap>();

        [JsonProperty("last_league_rank")]
        public int LastLeagueRank { get; set; }

        [JsonProperty("last_league_shuffle")]
        public int LastLeagueShuffle { get; set; }

        [JsonProperty("last_news_seen")]
        public int LastNewsSeen { get; set; }

        [JsonIgnore]
        public string Json
        {
            get => JsonConvert.SerializeObject(this, Settings);
            set
            {
                try
                {
                    var _Object = JObject.Parse(value);

                    Buildings.Clear();
                    foreach (var token in _Object["buildings"])
                    {
                        var building =
                            JsonConvert.DeserializeObject<Building>(JsonConvert.SerializeObject(token), Settings);

                        building.Id = building.Id <= 0
                            ? Buildings.Count > 0
                                ? Buildings.Max(d => d.Id) + 1
                                : 500000000
                            : building.Id;

                        Buildings.Add(building);
                    }

                    Traps.Clear();
                    foreach (var token in _Object["traps"])
                    {
                        var trap = JsonConvert.DeserializeObject<Trap>(JsonConvert.SerializeObject(token), Settings);

                        trap.Id = trap.Id <= 0
                            ? Traps.Count > 0
                                ? Traps.Max(d => d.Id) + 1
                                : 504000000
                            : trap.Id;

                        Traps.Add(trap);
                    }

                    Decorations.Clear();
                    foreach (var token in _Object["decos"])
                    {
                        var deco = JsonConvert.DeserializeObject<Decoration>(JsonConvert.SerializeObject(token),
                            Settings);

                        deco.Id = deco.Id <= 0
                            ? Decorations.Count > 0
                                ? Decorations.Max(d => d.Id) + 1
                                : 506000000
                            : deco.Id;

                        Decorations.Add(deco);
                    }

                    Obstacles.Clear();
                    foreach (var token in _Object["obstacles"])
                    {
                        var obstacle =
                            JsonConvert.DeserializeObject<Obstacle>(JsonConvert.SerializeObject(token), Settings);

                        obstacle.Id = obstacle.Id <= 0
                            ? Obstacles.Count > 0
                                ? Obstacles.Max(d => d.Id) + 1
                                : 508000000
                            : obstacle.Id;

                        Obstacles.Add(obstacle);
                    }
                }
                catch (Exception exception)
                {
                    if (Configuration.Debug)
                        Console.WriteLine(exception);
                }
            }
        }

        public void AddDeco(int id, int x, int y)
        {
            var globalId = Decorations.Count > 0 ? Decorations.Max(d => d.Id) + 1 : 506000000;

            var index = Decorations.FindIndex(deco => deco.Id == globalId);

            if (index != -1) return;
            Decorations.Add(new Decoration
            {
                Data = id,
                Id = globalId,
                X = x,
                Y = y
            });
        }

        public void AddTrap(int id, int x, int y)
        {
            var globalId = Traps.Count > 0 ? Traps.Max(t => t.Id) + 1 : 504000000;

            var index = Traps.FindIndex(trap => trap.Id == globalId);

            if (index != -1) return;
            Traps.Add(new Trap
            {
                Data = id,
                Id = globalId,
                X = x,
                Y = y,
                Level = 0
            });
        }

        public void AddBuilding(int id, int x, int y)
        {
            var globalId = Buildings.Count > 0 ? Buildings.Max(t => t.Id) + 1 : 500000000;

            var index = Buildings.FindIndex(building => building.Id == globalId);

            if (index != -1) return;

            Buildings.Add(new Building
            {
                Data = id,
                Id = globalId,
                X = x,
                Y = y,
                Level = 0
            });
        }

        public void Upgrade(int id)
        {
            if (id - 504000000 < 0)
            {
                var index = Buildings.FindIndex(building => building.Id == id);

                if (index > -1)
                    Buildings[index].Level++;
            }
            else
            {
                var index = Traps.FindIndex(trap => trap.Id == id);

                if (index > -1)
                    Traps[index].Level++;
            }
        }

        public void Move(int id, int x, int y)
        {
            if (id - 504000000 < 0)
            {
                var index = Buildings.FindIndex(building => building.Id == id);

                if (index <= -1) return;
                Buildings[index].X = x;
                Buildings[index].Y = y;
            }
            else if (id - 506000000 >= 0)
            {
                var index = Decorations.FindIndex(deco => deco.Id == id);

                if (index <= -1) return;
                Decorations[index].X = x;
                Decorations[index].Y = y;
            }
            else
            {
                var index = Traps.FindIndex(trap => trap.Id == id);

                if (index <= -1) return;
                Traps[index].X = x;
                Traps[index].Y = y;
            }
        }

        public void RemoveDeco(int id)
        {
            var index = Decorations.FindIndex(deco => deco.Id == id);

            if (index > -1)
                Decorations.RemoveAt(index);
        }

        public void RemoveObstacle(int id)
        {
            var index = Obstacles.FindIndex(deco => deco.Id == id);

            if (index > -1)
                Obstacles.RemoveAt(index);
        }

        public void SwitchAttackMode(int id)
        {
            if (id - 504000000 >= 0) return;

            var index = Buildings.FindIndex(building => building.Id == id);

            if (index <= -1) return;
            {
                var building = Buildings[index];
                building.AttackMode = !building.AttackMode;
            }
        }
    }
}