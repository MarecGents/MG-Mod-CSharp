using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace _MGMod.types.models.EFT.templetes;

public class BrothersItem : ICustomItemBuffs
{
    public string newId { get; set; }
    public string itemTplToClone { get; set; }
    public Props overrideProperties { get; set; }
    public double? fleaPriceRoubles { get; set; }
    public BroLocal locales { get; set; }
    public Dictionary<string, List<Buff>>? Buffs { get; set; }
}
public class BroLocal
{
    public LocaleDetails ch {  get; set; }
}
public class SuperItem : ICustomItemBuffs
{
    public string tpl {  get; set; }
    public SuperItems items { get; set; }
    public HandbookItem handbook { get; set; }
    public List<Item>? assort {  get; set; }
    public Dictionary<string, List<Buff>>? Buffs { get; set ; }
}
public class SuperItems
{
    public string _id { get; set; }
    public string _name { get; set; }
    public Props _props { get; set; }
    public string? _parent { get; set; }
    public string? _type { get; set; }
    public string? _proto { get; set; }
}
public class MGItem : ICustomItemBuffs
{
    public MGItems items { get; set; }
    public double price { get; set; }
    public LocaleDetails description { get; set; }
    public string? toTraderId { get; set; }
    public bool? isSold { get; set; }
    public int? loyal_level { get; set; }
    public List<Item>? assort { get; set; }
    public string? currency { get; set; }
    public Dictionary<string, List<Buff>>? Buffs { get; set; }
}
public class MGItems
{
    public string newId { get; set; }
    public string cloneId { get; set; }
    public Props _props { get; set; }
}
public class CustomTraderItems
{
    public TemplateItem item { get; set; }
    public string origin;
    public string[]? Type { get; set; }
}