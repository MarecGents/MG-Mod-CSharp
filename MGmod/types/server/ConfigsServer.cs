using _MGMod.types.models.Custom;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;

namespace _MGMod.types.server;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]

public class ConfigsServer(
    ConfigServer configServer,
    ISptLogger<ConfigsServer> logger
    )
{
    private AirdropConfig Airdrop => configServer.GetConfig<AirdropConfig>();
    private BackupConfig Backup => configServer.GetConfig<BackupConfig>();
    private BotConfig Bot => configServer.GetConfig<BotConfig>();
    private BtrDeliveryConfig BtrDelivery => configServer.GetConfig<BtrDeliveryConfig>();
    private CoreConfig Core => configServer.GetConfig<CoreConfig>();
    private GiftsConfig Gifts => configServer.GetConfig<GiftsConfig>();
    private HealthConfig Health => configServer.GetConfig<HealthConfig>();
    private HideoutConfig Hideout => configServer.GetConfig<HideoutConfig>();
    private HttpConfig Http => configServer.GetConfig<HttpConfig>();
    private InRaidConfig InRaid => configServer.GetConfig<InRaidConfig>();
    private InsuranceConfig Insurance => configServer.GetConfig<InsuranceConfig>();
    private InventoryConfig Inventory => configServer.GetConfig<InventoryConfig>();
    private ItemConfig Item => configServer.GetConfig<ItemConfig>();
    private LocaleConfig Locale => configServer.GetConfig<LocaleConfig>();
    private LocationConfig Location => configServer.GetConfig<LocationConfig>();
    private LootConfig Loot => configServer.GetConfig<LootConfig>();
    private LostOnDeathConfig LostOnDeath => configServer.GetConfig<LostOnDeathConfig>();
    private MatchConfig Match => configServer.GetConfig<MatchConfig>();
    private PlayerScavConfig PlayerScav => configServer.GetConfig<PlayerScavConfig>();
    private PmcChatResponse PmcChatResponse => configServer.GetConfig<PmcChatResponse>();
    private PmcConfig Pmc => configServer.GetConfig<PmcConfig>();
    private QuestConfig Quest => configServer.GetConfig<QuestConfig>();
    private RagfairConfig Ragfair => configServer.GetConfig<RagfairConfig>();
    private RepairConfig Repair => configServer.GetConfig<RepairConfig>();
    private ScavCaseConfig ScavCase => configServer.GetConfig<ScavCaseConfig>();
    private SeasonalEventConfig SeasonalEvent => configServer.GetConfig<SeasonalEventConfig>();
    private TraderConfig Trader => configServer.GetConfig<TraderConfig>();
    private WeatherConfig Weather => configServer.GetConfig<WeatherConfig>();


    // trader.json
    public void SetTradersUpdateTime(int min, int? max=null, string? traderId=null)
    {
        var seconds = new MinMax<int>
        {
            Max = max??min,
            Min = min
        };
        foreach (var key in Trader.UpdateTime)
        {
            if (string.IsNullOrEmpty(traderId) || key.TraderId == traderId)
            {
                key.Seconds = seconds;
            }
        }
    }

    // weather.json
    public void SetWeatherConfig(MGModConfig_Config_WeatherSettings value, string type = "default")
    {
        if (!Weather.Weather.PresetWeights.Keys.Contains(type)) return;
        var weather = Weather.Weather.PresetWeights[type];
        SetWeatherPresetWeightsType1(weather.Clouds, value.clouds);
        if(weather.WindSpeed != null) SetWeatherPresetWeightsType1(weather.WindSpeed, value.windSpeed);
        if(weather.Rain != null) SetWeatherPresetWeightsType1(weather.Rain, value.rain);
        if(weather.Fog != null) SetWeatherPresetWeightsType1(weather.Fog, value.fog);
    }

    public void SetWeatherPresetWeightsType1(Dictionary<string, double> presetWeights, MGModConfig_Config_Weather weatherType)
    {
        presetWeights.Clear();
		for (int ind = 0; ind < weatherType.values.Count; ind++)
		{
			presetWeights[weatherType.values[ind].ToString()] = weatherType.weights[ind];
		}
	}

    public void MGmodConfigs(MGModConfig_Config ConfigSetting)
    {
        // airdrop.json
        // 功能：空投种类 AirdropType
        if (ConfigSetting.AirdropType != "default")
        {
            var Type = ConfigSetting.AirdropType;
            var Weight = Airdrop.AirdropTypeWeightings;
            if(Type == "moreWeapon")
            {
                Weight[SptAirdropTypeEnum.weaponArmor] = 12;
            } 
            else if (Type == "moreFoodMedical")
            {
                Weight[SptAirdropTypeEnum.foodMedical] = 12;
            }
            else if (Type == "moreBarter")
            {
                Weight[SptAirdropTypeEnum.barter] = 12;
            }
            else if (Type == "moreMixed")
            {
                Weight[SptAirdropTypeEnum.mixed] = 9;
            }
        }

        // backup.json

        // bot.json
        // 功能：AI刷新数量 AISpawnNumber
        if (ConfigSetting.AISpawnNumber != 0)
        {
            foreach(var key in Bot.MaxBotCap.Keys)
            {
                Bot.MaxBotCap[key] += ConfigSetting.AISpawnNumber;
            }
        }

        // core.json
        // gifts.json
        // health.json
        // hideout.json
        // http.json

        // inraid.json
        // let inraid = MGConfigs.getConfig(ConfigTypes.IN_RAID);
        // 功能：战局默认选项 RaidDefault
        if (ConfigSetting.RaidDefault.enable)
        {
            InRaid.RaidMenuSettings.AiAmount = ConfigSetting.RaidDefault.aiAmount;
            InRaid.RaidMenuSettings.AiDifficulty = ConfigSetting.RaidDefault.aiDifficulty;
            InRaid.RaidMenuSettings.BossEnabled = ConfigSetting.RaidDefault.bossEnabled;
            InRaid.RaidMenuSettings.ScavWars = ConfigSetting.RaidDefault.scavWars;
            InRaid.RaidMenuSettings.TaggedAndCursed = ConfigSetting.RaidDefault.taggedAndCursed;
        }

        // insurance.json
        //功能：商人百分百回保 ReturnChance
        if(ConfigSetting.ReturnChance.enable)
        {
            var chance = ConfigSetting.ReturnChance.value;
            foreach(var key in Insurance.ReturnChancePercent.Keys)
            {
                Insurance.ReturnChancePercent[key] = chance;
            }
            Insurance.RunIntervalSeconds = 0;
        }

        // inventory.json
        // 功能：购买物品带钩 BuyFoundInRaid
        /*
        if (ConfigSetting.BuyFoundInRaid)
        {
            Inventory.NewItemsMarkedFound = ConfigSetting.BuyFoundInRaid;
        }
        */

        // item.json
        // locale.json

        // location.json
        // 功能：容器物资倍率 Container
        if (ConfigSetting.LootMultiple.Container != 1)
        {
            var multiplier = ConfigSetting.LootMultiple.Container;
            var staticsets = Location.StaticLootMultiplier;
            foreach(var key in staticsets.Keys)
            {
                staticsets[key] *= multiplier;
            }
        }
        // 功能：地面物资倍率 Ground
        if (ConfigSetting.LootMultiple.Ground != 1)
        {
            var multiplier = ConfigSetting.LootMultiple.Ground;
            var loosesets = Location.LooseLootMultiplier;
            foreach(var key in loosesets.Keys)
            {
                loosesets[key] *= multiplier;
            }
        }
        //功能：容器随机生成 RandomContainer
        if (ConfigSetting.RandomContainer)
        {
            Location.ContainerRandomisationSettings.Enabled = ConfigSetting.RandomContainer;
        }

        // loot.json
        // lostondeath.json
        // match.json
        // playerscav.json

        // pmc.json
        // 功能：USEC比例 USECRate
        if (ConfigSetting.USECRate.enable)
        {
            Pmc.IsUsec = ConfigSetting.USECRate.value;
        }

        // pmcchatresponse.json
        // quest.json

        // ragfair.json
        // 功能：跳蚤出售100% Sell100
        if (ConfigSetting.Sell100)
        {
            var RagfairSellChance = Ragfair.Sell.Chance;
            RagfairSellChance.Base = 100;
            RagfairSellChance.SellMultiplier = 2;
            RagfairSellChance.MaxSellChancePercent = 100;
            RagfairSellChance.MinSellChancePercent = 100;
        }
        // 功能：跳蚤极速出售 SellFast
        if (ConfigSetting.SellFast)
        {
            Ragfair.Sell.Time = new MinMax<double>
            {
                Max = 0.01,
                Min = 0
            };
        }
        // 功能：购买物品带钩 BuyFoundInRaid
        if (ConfigSetting.BuyFoundInRaid)
        {
            Ragfair.Dynamic.PurchasesAreFoundInRaid = ConfigSetting.BuyFoundInRaid;
        }
        // 功能：跳蚤购买优化 SellOptimize
        if (ConfigSetting.SellOptimize)
        {
            var RagfairDynamic = Ragfair.Dynamic;
            // 跳蚤不可堆叠物品出售数量
            RagfairDynamic.NonStackableCount = new MinMax<int>
            {
                Max = 5000,
                Min = 100
            };
            // 跳蚤可堆叠物品出售数量
            RagfairDynamic.StackablePercent = new MinMax<double>
            {
                Max = 50000,
                Min = 500
            };
            // // 跳蚤显示为单个物品的
            RagfairDynamic.ShowAsSingleStack = new HashSet<MongoId> { };
            // 护甲没有插板概率
            RagfairDynamic.Armor.RemoveRemovablePlateChance = 0;
        }
        // 功能：跳蚤物品全新 SellNew
        if (ConfigSetting.SellNew)
        {
            var RagfairDynamicCondition = Ragfair.Dynamic.Condition;
            foreach (var key in RagfairDynamicCondition.Keys)
            {
                RagfairDynamicCondition[key].ConditionChance = 0;
            }
        }
        // 功能：禁用跳蚤黑名单 NoBlackList
        if (ConfigSetting.NoBlackList)
        {
            Ragfair.Dynamic.Blacklist.EnableBsgList = !ConfigSetting.NoBlackList;
        }

        // repair.json
        // 功能：护甲附魔
        if (ConfigSetting.Buffs.BuffsArmor)
        {
            var RepairKit = Repair.RepairKit;
            var RarityWeight = new Dictionary<string, double>
            {
                { "Common", 0},
                { "Rare", 100}
            };
            RepairKit.Armor.RarityWeight = RarityWeight;
            RepairKit.Vest.RarityWeight = RarityWeight;
            RepairKit.Headwear.RarityWeight = RarityWeight;
            Repair.ArmorKitSkillPointGainPerRepairPointMultiplier *= 100;
        }
        // 功能：武器附魔
        if (ConfigSetting.Buffs.BuffsWeapon)
        {
            var RepairKit = Repair.RepairKit;
            var RarityWeight = new Dictionary<string, double>
            {
                { "Common", 0},
                { "Rare", 100}
            };
            RepairKit.Weapon.RarityWeight = RarityWeight;
            //Repair.WeaponSkillRepairGain *= 100;
        }
        // 功能：附魔
        if (ConfigSetting.Buffs.BuffsWeapon || ConfigSetting.Buffs.BuffsArmor)
        {
            Repair.RepairKitIntellectGainMultiplier.Weapon = 100;
            Repair.RepairKitIntellectGainMultiplier.Armor = 100;
            Repair.MaxIntellectGainPerRepair.Kit = 1;
            Repair.MaxIntellectGainPerRepair.Trader = 1;
        }

        // scavcase.json
        // seasonalevents.json

        // trader.json
        // 功能：商人供货时间 UpdateTime
        if (ConfigSetting.UpdateTime.enable)
        {
            var updateTime = ConfigSetting.UpdateTime.value;
            Trader.UpdateTimeDefault = updateTime;
            SetTradersUpdateTime(updateTime);
        }
        // 功能：购买物品带钩 BuyFoundInRaid
        if (ConfigSetting.BuyFoundInRaid)
        {
            Trader.PurchasesAreFoundInRaid = ConfigSetting.BuyFoundInRaid;
        }

        // weather.json
        // 功能：天气修改
        if (ConfigSetting.WeatherSettings.mode != "default")
        {
            HashSet<String> weatherType = ["SUNNY","RAINY","CLOUDY","WINTER"];
            foreach (var weather in weatherType)
            {
                SetWeatherConfig(ConfigSetting.WeatherSettings, weather);
            }
        }
    }
}
