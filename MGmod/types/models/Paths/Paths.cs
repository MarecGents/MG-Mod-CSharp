namespace _MGMod.types.models.Paths;

public static class Paths
{
    public static readonly PathType PriceJson = new ()
    {
        FileName = "price.json",
        Path = "./res/price"
    };

    public static readonly PathType GithubToken = new()
    {
        FileName = "token.json",
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