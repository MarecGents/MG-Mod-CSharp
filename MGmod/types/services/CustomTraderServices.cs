using _MGMod.types.models.EFT.traders;
using _MGMod.types.models.Paths;
using _MGMod.types.server;
using _MGMod.types.utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Loaders;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Routers;
using Path = System.IO.Path;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class CustomTraderServices
{
    private MGUtils mGUtils;
    private ISptLogger<CustomTraderServices> logger;
    private ImageRouter  imageRouter;
    private LocalesServer localesServer;
    private ConfigsServer configsServer;
    private TemplatesServer templatesServer;
    
    public CustomTraderServices(
        MGUtils _mGUtils,
        ISptLogger<CustomTraderServices> _logger,
        ImageRouter _imageRouter,
        LocalesServer _localesServer,
        ConfigsServer _configsServer,
        TemplatesServer _templatesServer
    )
    {
        mGUtils = _mGUtils;
        logger = _logger;
        imageRouter = _imageRouter;
        localesServer = _localesServer;
        configsServer = _configsServer;
        templatesServer = _templatesServer;
    }
    
    
    public void Start()
    {
        var Bundles = AddTraders();
        AddBundles(Bundles);
    }

    public BundleManifest AddTraders()
    {
        var TradersDirectories = mGUtils.GetDirectories(Paths.Traders);
        BundleManifest Bundles = new BundleManifest();
        foreach (var traderName in TradersDirectories)
        {
            string traderPath = Path.Combine(Paths.Traders, traderName);
            bool addFlag = AddTraderBaseToDB(traderPath);
            if (!addFlag) continue;
            
            logger.LogWithColor($"[MGMod][独立商人]商人【{traderName}】已添加。");
        }
        return Bundles;
    }

    public void AddBundles(BundleManifest Bundles)
    {
        
    }

    public bool AddTraderBaseToDB(string traderPath)
    {
        CustomTraderInfo traderInfo = mGUtils.GetJsonDataFromFile<CustomTraderInfo>(new PathType
        {
            FileName = TraderPathsType.TraderInfo,
            Path = traderPath,
        });
        var returnFlag = 0;
        if (traderInfo == default)
        {
            // 2026.01.19 23:14 进度于此
            logger.LogWithColor($"[MGMod][独立商人]商人{Path.GetFileName(traderPath)}不存在配置文件\"traderInfo.json\"，请检查商人文件完整性。",LogTextColor.Cyan);
            returnFlag = returnFlag + 1;
        }
        
        var Traders = templatesServer.GetTraders();
        if (Traders.ContainsKey(traderInfo._id))
        {
            logger.LogWithColor($"[MGMod][独立商人]商人{Path.GetFileName(traderPath)}的Id:{traderInfo._id}已存在于游戏中,请修改。",LogTextColor.Cyan);
            returnFlag = returnFlag + 1;
        }
        
        if (returnFlag != 0) return false;
        if (!MongoId.IsValidMongoId(traderInfo._id)) 
            logger.LogWithColor($"[MGMod][独立商人]商人{traderInfo.name}的Id:{traderInfo._id}不符合MongoId格式，请酌情修改。【如果你安装了无视MongoId限制的Mod，可忽视本条消息】",LogTextColor.Cyan);
        
        if (!traderInfo.enable) return false;

        TraderBase traderBase = mGUtils.GetJsonDataFromFile<TraderBase>(new PathType
        {
            FileName = "base.json",
            Path = Paths.TraderDB,
        });
        
        // base.json
        traderBase.Id = traderInfo._id;
        traderBase.Name = traderInfo.locales.FullName;
        traderBase.Surname = traderInfo.locales.FirstName;
        traderBase.Nickname = traderInfo.locales.Nickname;
        traderBase.Location = traderInfo.locales.Location;
        traderBase.Insurance.Availability = traderInfo.insurance?.enable;
        traderBase.Insurance.MinPayment = traderInfo.insurance?.pay;
        traderBase.Insurance.MinReturnHour =  traderInfo.insurance?.minreturntime;
        traderBase.Insurance.MaxReturnHour = traderInfo.insurance?.maxreturntime;
        traderBase.Insurance.MaxStorageTime = traderInfo.insurance?.storageTime;
        traderBase.Repair.Availability = traderInfo.repair?.enable;
        traderBase.Repair.CurrencyCoefficient = traderInfo.repair?.coefficient;
        traderBase.Repair.Quality = traderInfo.repair?.quality;
        traderBase.Medic = traderInfo.medic;
        traderBase.LoyaltyLevels = traderInfo.loyaltyLevels?.range;
        traderBase.Discount = traderInfo.discount;
        traderBase.UnlockedByDefault = traderInfo.unlockedDefault;
        
        Trader newTrader = new Trader
        {
            Assort = new TraderAssort
            {
                Items = new(),
                BarterScheme = new(),
                LoyalLevelItems = new()
            },
            Base = traderBase,
            Dialogue = new Dictionary<string, List<string>?>
            {
                ["insuranceStart"] =[],
                ["insuranceFound"] = [],
                ["insuranceFailed"] = [],
                ["insuranceExpired"] = [],
                ["insuranceComplete"] = [],
                ["insuranceFailedLabs"] = []
                
            },
            QuestAssort = new Dictionary<string, Dictionary<MongoId, MongoId>>
            {
                ["started"] = new (){},
                ["success"] = new (){},
                ["fail"] = new (){},
            }
        };
        
        // dialogue.json
        foreach (var dialogue in newTrader.Dialogue)
        {
            if (traderInfo.insurance.Message.TryGetValue(dialogue.Key, out var message))
            {
                dialogue.Value?.AddRange(message);
            }
        }
        // questassort.json
        Dictionary<string, Dictionary<MongoId, MongoId>> traderQuestAssort = mGUtils.GetJsonDataFromFile<Dictionary<string, Dictionary<MongoId, MongoId>>>(new PathType
        {
            FileName = "questassort.json",
            Path = Path.Combine(traderPath,TraderPathsType.TraderDataPath),
        });
        // assort.json
        TraderAssort traderAssort = mGUtils.GetJsonDataFromFile<TraderAssort>(new PathType
        {
            FileName = "assort.json",
            Path = Path.Combine(traderPath,TraderPathsType.TraderDataPath),
        });
        if (traderAssort.Items.Count > 0)
        {
            foreach (var item in traderAssort.Items)
            {
                if(mGUtils.IsMongoId(item.Id)) continue;
                MongoId newId = new MongoId();
                traderAssort = mGUtils.ReplaceKey(traderAssort, item.Id, newId);
                traderQuestAssort = mGUtils.ReplaceKey(traderQuestAssort, item.Id, newId);
            }
            newTrader.Assort.Items.AddRange(traderAssort.Items);
            foreach (var item in traderAssort.BarterScheme)
            {
                newTrader.Assort.BarterScheme.Add(item.Key, item.Value);
            }
            foreach (var item in traderAssort.LoyalLevelItems)
            {
                newTrader.Assort.LoyalLevelItems.Add(item.Key, item.Value);
            }

            foreach (var key in traderQuestAssort)
            {
                foreach (var key2 in key.Value)
                {
                    newTrader.QuestAssort[key.Key].Add(key2.Key, key2.Value);
                }
            }
        }

        // image
        string traderImage = $"{traderInfo._id}.jpg";
        string imagePath = Path.Combine(traderPath,traderImage);
        if (mGUtils.FileExists(imagePath))
        {
            newTrader.Base.Avatar = newTrader.Base.Avatar.Replace("000000000000000000000000.jpg", traderImage);
            imageRouter.AddRoute(newTrader.Base.Avatar.Replace(".jpg",""), imagePath);
        }
        else logger.LogWithColor($"[MGMod][独立商人]{traderInfo.name}：混蛋，你把我的头像放哪了！快还给我！", LogTextColor.Cyan);

        if (!Traders.TryAdd(traderInfo._id, newTrader))
        {
            logger.LogWithColor($"[MGMod][独立商人]{traderInfo.name}：添加失败，发生了未知错误", LogTextColor.Cyan);
            return false;
        }
        
        // locales
        localesServer.AddTraderInfo(new _MGMod.types.models.EFT.locales.TraderInfo
        {
            Id = traderInfo._id,
            Desc = traderInfo.locales
        });
        
        // insurance.json
        if (traderInfo.insurance.enable) configsServer.AddTraderReturnChance(traderInfo._id, traderInfo.insurance.chance);
        // quest.json
        configsServer.AddRepeatableQuestTraderWhitelist(traderInfo._id, traderInfo.locales.Nickname);
        // ragfair.json
        configsServer.AddTraderRagfair(traderInfo._id);
        // traders.json
        configsServer.SetTradersUpdateTime(traderInfo.updateTime.Min,traderInfo.updateTime.Max, traderInfo._id, traderInfo.locales.Nickname);

        templatesServer.AddTraderInitLoyaltyLevel(traderInfo._id);

        return true;
    }

    public void AddImageToDB(string traderPath)
    {
        
    }

    public void AddTraderItemsToDB(string traderPath)
    {
        
    }

    public void AddTraderLocalesToDB(string traderPath)
    {
        
    }

    public void AddTraderLocationToDB(string traderPath)
    {
        
    }

    public void AddTraderTemplatesToDB(string traderPath)
    {
        
    }

    public void AddTraderBundlesToDB(string traderPath)
    {
        
    }

    public void AddTraderGlobalsToDB(string traderPath)
    {
        
    }
    
}