namespace _MGMod.types.models.Paths;

public static class Paths
{
    public static readonly PathType BundlesJson = new ()
    {
        FileName = "bundles.json",
        Path = ""
    };
    
    public static readonly PathType PriceJson = new ()
    {
        FileName = "price.json",
        Path = "./res/price"
    };

    public static readonly PathType ConfigJson = new()
    {
        FileName = "config.json",
        Path = "./res/config"
    };

    public static readonly PathType MapChNameJson = new()
    {
        FileName = "MapChName.json",
        Path = "./res/Keys"
    };

    public static readonly PathType MapKeyJson = new()
    {
        FileName = "MapKey.json",
        Path = "./res/Keys"
    };

    public static readonly PathType MapZoneFullJson = new()
    {
        FileName = "MapZoneFull.json",
        Path = "./res/locations"
    };

    public static readonly PathType QuestKeyJson = new()
    {
        FileName = "QuestKey.json",
        Path = "./res/Keys"
    };
    
    public static readonly PathType ProfileJson = new()
    {
        FileName = "profile.json",
        Path = "./res/profile"
    };
    
    public static readonly PathType Quest3X4Json = new()
    {
        FileName = "quest3X4.json",
        Path = "./res/quest"
    };
    
    public static readonly PathType GunSmithJson = new()
    {
        FileName = "GunSmith.json",
        Path = "./res/profile"
    };
    
    public static readonly PathType GPNVGJson = new()
    {
        FileName = "GPNVG.json",
        Path = "./res/services/itemsDB"
    };

    public static readonly PathType T7Json = new()
    {
        FileName = "T7.json",
        Path = "./res/services/itemsDB"
    };

    public static readonly PathType LooseLootJson = new()
    {
        FileName = "looseLoot.json",
        Path = "./res/services/locationsDB"
    };

    public static readonly string TraderDB = "./res/services/TraderDB";

    public static readonly string TradersPackage = "./traders";

    public static readonly string MGItemDB = "./db/MGItem";

    public static readonly string BrothersItemDB = "./db/BrothersItem/";

    public static readonly string SuperItemPath = "./db/SuperModItem/";

    public static readonly string AssortItemPath = "./db/assort/";
    
    public static readonly string TestPath = "./db/test/";

    public static readonly string Traders = "./traders";
    
    public static readonly string SPTServerLocalsPath = "../../../SPT_Data/database/locales/server/";
}

public static class TraderPathsType
{
    public static readonly string TraderImagesPath = "./images";
    public static readonly string TraderQuestImagesPath = "./images/quests";
    public static readonly string TraderItemsPath = "./items";
    public static readonly string TraderLocalesPath = "./locales";
    public static readonly PathType TraderItemDesc = new ()
    {
        FileName = "itemsdescription.json",
        Path = TraderLocalesPath
    };
    public static readonly PathType TraderMail = new ()
    {
        FileName = "mail.json",
        Path = TraderLocalesPath
    };
    public static readonly string TraderLocationPath = "./location";
    public static readonly PathType TraderLooseLoot = new ()
    {
        FileName = "looseLoot.json",
        Path = TraderLocationPath
    };
    public static readonly string TraderTemplatesPath = "./templates";
    public static readonly PathType TraderHandbook = new ()
    {
        FileName = "handbook.json",
        Path = TraderTemplatesPath
    };
    public static readonly PathType TraderQuests = new ()
    {
        FileName = "quests.json",
        Path = TraderTemplatesPath
    };
    public static readonly string TraderDataPath = "./traderData";
    public static readonly string TraderBundles = "bundles.json";
    public static readonly string TraderGlobals = "globals.json";
    public static readonly string TraderInfo = "traderInfo.json";
}

public static class BotSystemPathsType
{
    public static readonly string BotSystemPath = "./res/botsystem/";
    public static readonly string PmcRoarPath = "./res/botsystem/pmcroar/";
    public static readonly string MapBotDifficultyPath = "./res/botsystem/mapbotdifficulty/";

    public static readonly PathType EasyMapPath = new()
    {
        FileName = "easymap.json",
        Path = MapBotDifficultyPath
    };
    public static readonly PathType HardMapPath = new()
    {
        FileName = "hardmap.json",
        Path = MapBotDifficultyPath
    };

    public static readonly PathType PmcNamePath = new()
    {
        FileName =  "pmcname.json",
        Path = BotSystemPath
    };
    
    public static readonly PathType PmcTacticalSquadPath = new()
    {
        FileName =  "pmctacticalsquad.json",
        Path = BotSystemPath
    };

}

public class PathType
{
    public required string FileName { get; set; }
    public required string Path { get; set; }

    public PathType()
    {
        
    }
    public PathType(string fileName, string path)
    {
        FileName = fileName;
        Path = path;
    }
}