using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;

namespace _MGMod.types.models.EFT.traders;

public class CustomItemAssorts
{
    public List<Item> assort { get; set; }
    public double price { get; set; }
    public int loyal_level_items { get; set; }
    public MongoId traderId { get; set; }
    public MongoId currency { get; set; }
}
