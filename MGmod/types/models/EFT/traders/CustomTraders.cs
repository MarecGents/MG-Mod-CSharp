using _MGMod.types.models.EFT.locales;
using _MGMod.types.models.EFT.locations;
using _MGMod.types.models.EFT.templetes;
using SPTarkov.Server.Core.Loaders;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;

namespace _MGMod.types.models.EFT.traders;

public class CustomTraders
{
    public Dictionary<string, CustomTraderItems>? items {  get; set; }
    public CustomTraderLocales? locales { get; set; }
    public CustomTraderLocation? location { get; set; }
    public CustomTraderTemplates? templates { get; set; }
    public Trader? traderData { get; set; }
    public BundleManifest? bundles { get; set; }
    public CustomGlobals? globals { get; set; }
}

public class CustomTraderLocales
{
    public Dictionary<string, ItemsDesc>? itemsdescription { get; set; }
    public Dictionary<string, QuestDesc>? mail { get; set; }
}
public class CustomTraderLocation
{
    public Dictionary<string, CustomLooseLoot>? looseLoot { get; set; }
}

public class CustomTraderTemplates
{
    public List<HandbookItem>? handbook { get; set; }
    public Dictionary<string, Quest>? quests { get; set; }
}

public class CustomTraderInfo
{
    public bool enable {  get; set; }
    public string _id { get; set; }
    public string name { get; set; }
    public TraderDesc locales {  get; set; }
    
}

public class CustomTraderInsurance
{
    public bool enable { get; set; }
    public int minreturntime { get; set; }
    public int maxreturntime { get; set; }
    public double pay { get; set; }
    public int chance { get; set; }
    public int storageTime { get; set; }
    public CustomTraderInsuranceMessage? Message { get; set; }
    public CustomTraderRepair? reapir { get; set; }

}

public class CustomTraderInsuranceMessage
{
    public List<string>? insuranceStart { get; set; }
    public List<string>? insuranceFound { get; set; }
    public List<string>? insuranceFailed { get; set; }
    public List<string>? insuranceExpired { get; set; }
    public List<string>? insuranceComplete { get; set; }
    public List<string>? insuranceFailedLabs { get; set; }
}

public class CustomTraderRepair
{
    public bool enable { get; set; }
    public int coefficient { get; set; }
    public int quality { get; set; }
}

public class CustomTraderLoyaltyLevels
{
    public Dictionary<string,string>? description { get; set; }

}

