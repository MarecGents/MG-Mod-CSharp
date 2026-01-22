using _MGMod.types.models.Custom;
using _MGMod.types.models.EFT.locations;
using _MGMod.types.utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Server;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace _MGMod.types.server;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class LocationsServer
{
    private DatabaseService databaseService;
    private ISptLogger<LocationsServer> logger;
    private MGUtils mGUtils;
    private Locations Locations => databaseService.GetLocations();
    
    public LocationsServer(
        DatabaseService _databaseService,
        ISptLogger<LocationsServer> _logger,
        MGUtils _mGUtils
    )
    {
        databaseService =  _databaseService;
        logger =  _logger;
        mGUtils =  _mGUtils;
    }
    
    public Dictionary<string, Location> GetLocations()
    {
        return Locations.GetDictionary();
    }

    // public void AddLooseLootByMapName(CustomLooseLoot customLooseLoot, string mapName)
    // {
    //     LooseLoot looseloot = GetLocations()[mapName].LooseLoot.Value;
    //     if (customLooseLoot.spawnpointCount?.Mean != null && customLooseLoot.spawnpointCount?.Std != null)
    //     {
    //         looseloot.SpawnpointCount.Mean  = customLooseLoot.spawnpointCount.Mean;
    //         looseloot.SpawnpointCount.Std  = customLooseLoot.spawnpointCount.Std;
    //     }
    //
    //     if (customLooseLoot.spawnpoints.Count > 0)
    //     {
    //         var looselootList = looseloot.Spawnpoints?.ToList();
    //         looselootList.AddRange(customLooseLoot.spawnpoints);
    //         looseloot.Spawnpoints = looselootList;
    //     }
    //     if (customLooseLoot.spawnpointsForced.Count > 0)
    //     {
    //         var looselootList = looseloot.SpawnpointsForced?.ToList();
    //         looselootList.AddRange(customLooseLoot.spawnpointsForced);
    //         looseloot.SpawnpointsForced = looselootList;
    //     }
    // }
    //
    // public CustomLooseLoot ResetLooseLoot(CustomTraderLooseLoot customLooseLoot)
    // {
    //     CustomLooseLoot  looseLoot = new CustomLooseLoot
    //     {
    //         spawnpointCount = null,
    //         spawnpoints = [],
    //         spawnpointsForced = []
    //     }; 
    //     looseLoot.spawnpointCount = customLooseLoot.spawnpointCount;
    //     foreach (var spawnpoint in customLooseLoot.spawnpoints)
    //     {
    //         List<CustomSptLootItem> newItems = new List<CustomSptLootItem>();
    //         newItems.AddRange(spawnpoint.template.Items);
    //         var newItemsString = mGUtils.Serialize(newItems);
    //         foreach (var item in spawnpoint.template.Items)
    //         {
    //             mGUtils.TestOutput("003");
    //             mGUtils.TestOutput(item);
    //             if(mGUtils.IsMongoId(item._id)) continue;
    //             MongoId newId = mGUtils.Generate();
    //             newItemsString = mGUtils.ReplaceKey(newItemsString,  item._id, newId);
    //         }
    //
    //         newItems = mGUtils.Deserialize<List<CustomSptLootItem>>(newItemsString);
    //         mGUtils.TestOutput("004");
    //         spawnpoint.template.Items.Clear();
    //         spawnpoint.template.Items.AddRange(newItems);
    //         mGUtils.TestOutput("005");
    //         // Spawnpoint newSpawnpoint = mGUtils.Deserialize<Spawnpoint>(mGUtils.Serialize(spawnpoint));
    //         // looseLoot.spawnpoints.Add(newSpawnpoint);
    //     }
    //     
    //     foreach (var spawnpointForced in customLooseLoot.spawnpointsForced)
    //     {
    //         mGUtils.TestOutput("006");
    //         List<CustomSptLootItem> newItems = new List<CustomSptLootItem>();
    //         newItems.AddRange(spawnpointForced.template.Items);
    //         foreach (var item in newItems)
    //         {
    //             mGUtils.TestOutput("007");
    //             if(mGUtils.IsMongoId(item._id)) continue;
    //             MongoId newId = new MongoId();
    //             newItems = mGUtils.ReplaceKey(newItems,  item._id, newId);
    //         }
    //         
    //         spawnpointForced.template.Items.Clear();
    //         spawnpointForced.template.Items.AddRange(newItems);
    //         mGUtils.TestOutput("008");
    //         Spawnpoint newSpawnpointForced = mGUtils.Deserialize<Spawnpoint>(mGUtils.Serialize(spawnpointForced));
    //         looseLoot.spawnpointsForced.Add(newSpawnpointForced);
    //     }
    //     
    //     return looseLoot;
    // }
    public void MGmodLocations(MGModConfig_Locations LocationsSetting)
    {
        var Locations_ = Locations.GetDictionary();
        string[] Exclude = [ "Develop", "Hideout", "PrivateArea", "Suburbs", "Terminal", "Town"];
        foreach (var mapName  in Locations_.Keys)
        {
            if (Exclude.Contains(mapName)) continue;
            // 功能：战局时长(分钟) RaidTime
            if (LocationsSetting.RaidTime.enable)
            {
                Locations_[mapName].Base.EscapeTimeLimit = LocationsSetting.RaidTime.value;
            }
            // 功能：BOSS刷新率 BOSSSpwanChance
            if (LocationsSetting.BOSSSpwanChance.enable && Locations_[mapName].Base.BossLocationSpawn != null)
            {
                foreach(var Bzone in Locations_[mapName].Base.BossLocationSpawn)
                {
                    if (Bzone.BossName.IndexOf("boss") == 0 || Bzone.Supports != null)
                    {
                        Bzone.BossChance = LocationsSetting.BOSSSpwanChance.value;
                    }
                }
            }
            // 功能：100%可拉闸  功能：100%可撤离
            if ((LocationsSetting.Pass100 || LocationsSetting.Escape100) && Locations_[mapName].Base.Exits != null)
            {
                foreach(var exit in Locations_[mapName].Base.Exits)
                {
                    if (exit.PassageRequirement == null) continue;
                    if (exit.PassageRequirement == RequirementState.WorldEvent && LocationsSetting.Pass100)
                    {
                        exit.Chance = 100;
                    }
                    else if (LocationsSetting.Escape100)
                    {
                        exit.Chance = 100;
                    }

                }
            }
            // 功能：地图是否回保 MapInsurance
            if (LocationsSetting.MapInsurance.ContainsKey(mapName))
            {
                Locations_[mapName].Base.Insurance = LocationsSetting.MapInsurance[mapName];
                Locations_[mapName].Base.IsSecret = !LocationsSetting.MapInsurance[mapName];
            }
            
        }
    }
}
