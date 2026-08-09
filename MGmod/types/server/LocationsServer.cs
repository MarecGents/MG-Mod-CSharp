using _MGMod.types.models.Custom;
using _MGMod.types.models.EFT.locations;
using _MGMod.types.models.EFT.templetes;
using _MGMod.types.models.Paths;
using _MGMod.types.utils;
using SPTarkov.Common.Logger;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Tables;

namespace _MGMod.types.server;

[Injectable(TypePriority = OnLoadOrder.Preload + 1)]
public class LocationsServer(
    LocationTable Locations,
    MGUtils mGUtils
    )
{
    public Dictionary<string, Location> GetLocations()
    {
        return Locations.GetDictionary();
    }
    
    public void MGmodLocations(MGModConfig_Locations LocationsSetting)
    {
        var Locations_ = GetLocations();
        Dictionary<string,MapBotDifficulty> easyMapJson = mGUtils.GetJsonDataFromFile<Dictionary<string,MapBotDifficulty>>(BotSystemPathsType.EasyMapPath);
        Dictionary<string,MapBotDifficulty> hardMapJson = mGUtils.GetJsonDataFromFile<Dictionary<string,MapBotDifficulty>>(BotSystemPathsType.HardMapPath);
        Dictionary<string, List<string>> mapZoneFull =
            mGUtils.GetJsonDataFromFile<Dictionary<string, List<string>>>(Paths.MapZoneFullJson);
        PmcTacticalSquadType pmcTacticalSquad =
            mGUtils.GetJsonDataFromFile<PmcTacticalSquadType>(BotSystemPathsType.PmcTacticalSquadPath);
        string[] Exclude = [ "Develop", "Hideout", "PrivateArea", "Suburbs", "Terminal", "Town"];
        List<string> hardLevelMapList = ["Laboratory", "Labyrinth", "SandboxHigh", "TarkovStreets"];
        List<string> smallMapList = ["Bigmap", "Factory4Day", "Factory4Night", "Laboratory", "Sandbox", "SandboxHigh"]; 
        List<string> bossNames = [
            "bossBully",
            "bossKnight",
            "bossPartisan",
            "bossTagilla",
            "bossKilla",
            "bossZryachiy",
            "bossGluhar",
            "bossSanitar",
            "bossBoar",
            "bossBoarSniper",
            "bossKolontay",
            "bossTagillaAgro",
            "tagillaHelperAgro",
            "bossKojaniy",
            "bossKillaAgro"
        ];
        
        foreach (var mapName  in Locations_.Keys)
        {
            if (Exclude.Contains(mapName)) continue;
            var locationBase = Locations_[mapName].Base;
            // 功能：战局时长(分钟) RaidTime
            if (LocationsSetting.RaidTime.enable)
            {
                locationBase.EscapeTimeLimit = LocationsSetting.RaidTime.value;
            }
            // 功能：BOSS刷新率 BOSSSpwanChance
            if (LocationsSetting.BOSSSpwanChance.enable && locationBase.BossLocationSpawn.Count > 0)
            {
                foreach(var Bzone in locationBase.BossLocationSpawn)
                {
                    if (!bossNames.Contains(Bzone.BossName)) continue;
                    Bzone.BossChance = LocationsSetting.BOSSSpwanChance.value;
                }
            }
            // 功能：100%可拉闸  功能：100%可撤离
            if ((LocationsSetting.Pass100 || LocationsSetting.Escape100) && locationBase.Exits.Any())
            {
                foreach(var exit in locationBase.Exits)
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
                locationBase.Insurance = LocationsSetting.MapInsurance[mapName];
                locationBase.IsSecret = !LocationsSetting.MapInsurance[mapName];
            }
            // 功能：地图AI难度分布 MapBotDifficulty : easy, normal, hard, impossible
            if (LocationsSetting.BotSystem.MapBotDifficulty != "default")
            {
                  string difficulty = LocationsSetting.BotSystem.MapBotDifficulty;
                  foreach(var Bzone in locationBase.BossLocationSpawn)
                  {
                      Bzone.BossDifficulty = difficulty;
                      Bzone.BossEscortDifficulty = difficulty;
                  }
                  MapBotDifficulty mapBotDifficulty = easyMapJson[difficulty];
                  if (hardLevelMapList.Contains(mapName))
                  {
                      mapBotDifficulty = hardMapJson[difficulty];
                  }
                  locationBase.BotEasy = mapBotDifficulty.BotEasy;
                  locationBase.BotNormal = mapBotDifficulty.BotNormal;
                  locationBase.BotHard = mapBotDifficulty.BotHard;
                  locationBase.BotImpossible = mapBotDifficulty.BotImpossible;
            }
            // 功能：PMC战术小队 PmcTacticalSquad
            if (LocationsSetting.BotSystem.PmcTacticalSquad)
            {
                List<string> zoneList = new List<string>();
                foreach (var zones in mapZoneFull[mapName])
                {
                    if(zones.Contains("Snipe")) continue;
                    zoneList.Add(zones);
                }

                BossLocationSpawn usecSpawn = mGUtils.Deserialize<BossLocationSpawn>(mGUtils.Serialize(pmcTacticalSquad.USEC));
                BossLocationSpawn bearSpawn = mGUtils.Deserialize<BossLocationSpawn>(mGUtils.Serialize(pmcTacticalSquad.BEAR));
                string zoneString = string.Join(",", zoneList);
                usecSpawn.BossZone = zoneString;
                bearSpawn.BossZone = zoneString;
                locationBase.BossLocationSpawn.Add(usecSpawn);
                locationBase.BossLocationSpawn.Add(bearSpawn);
            }
            // 功能：地图刷新参数优化 MapRefershConfig
            if (LocationsSetting.BotSystem.MapRefershConfig)
            {
                bool flag = false;
                if (smallMapList.Contains(mapName)) flag = true;
                // locationBase.SavSummonSeconds;
                // locationBase.MatchingMinSeconds;
                // locationBase.UsersSpawnSecondsN;
                // locationBase.UsersSpawnSecondsN2;
                locationBase.MaxBotPerZone = flag?3:7;
                locationBase.MaxDistToFreePoint = flag?400:800;
                locationBase.MinDistToFreePoint = flag?30:60;
                locationBase.BotSpawnTimeOffMax = flag?30:40;
                locationBase.BotSpawnTimeOffMin = flag?20:30;
                locationBase.BotSpawnTimeOnMax = flag?10:50;
                locationBase.BotSpawnTimeOnMin = 0;
                // locationBase.BotMaxTimePlayer;
                // locationBase.BotMaxPlayer;
                locationBase.BotStart = 0;
                locationBase.BotStop = 60*(int)locationBase.EscapeTimeLimit;
            }
        }
    }
}
