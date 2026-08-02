using System.Runtime.CompilerServices;
using _MGMod.types.models.EFT.templetes;
using _MGMod.types.models.EFT.traders;
using _MGMod.types.models.Paths;
using _MGMod.types.server;
using _MGMod.types.utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using Color = Spectre.Console.Color;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.Preload + 1)]
public class CustomAssortServices(
    MGUtils mGUtils,
    TradersServer tradersServer
    )
{
    public void Start()
    {
        var count = AddAssortsToTraders(GetAssorts());
        Log($"已添加{count}个独立预设。",Color.Yellow);
    }

    private Dictionary<string, CustomItemAssorts> GetAssorts()
    {
        Dictionary<string, CustomItemAssorts> customAssorts = new Dictionary<string, CustomItemAssorts>();
        List<string> assortFiles = mGUtils.GetFiles(Paths.AssortItemPath);
        if (assortFiles.Count == 0) return customAssorts;
        foreach (var assortFile in assortFiles)
        {
            if(!mGUtils.FileExists(assortFile, false)) continue;
            var fileName = mGUtils.StripExtension(assortFile);
            var assort = mGUtils.GetJsonDataFromFile<CustomItemAssorts>(new PathType()
            {
                Path = Paths.AssortItemPath,
                FileName = fileName + ".json"
            });
            if (assort == null) continue;
            customAssorts.TryAdd(fileName, assort);
        }
        return customAssorts;
    }
    
    private int AddAssortsToTraders(Dictionary<string, CustomItemAssorts> customAssorts)
    {
        int count = 0;
        foreach (var assort in customAssorts)
        {
            tradersServer.AddAssortsToTrader(assort.Value);
            count++;
        }
        return count;
    }
    
    public CustomItemAssorts CreateCustomItemAssorts(MGItem item)
    {
        var customAssorts = new CustomItemAssorts()
        {
            assort = new List<Item>(),
            currency = item.currency?? new MongoId(Money.ROUBLES),
            loyal_level_items = item.loyal_level??1,
            price = item.price,
            traderId = item.toTraderId ?? new MongoId("8ef5b2eff000000000000000")

        };
        if (item.assort.Count > 0)
        {
            customAssorts.assort = FixAssort(item.assort);
        }
        else
        {
            customAssorts.assort = new List<Item>()
            {
                new Item {
                    Id = new MongoId(),
                    Template = item.items.newId,
                    ParentId = "hideout",
                    SlotId = "hideout",
                    Upd = new Upd()
                    {
                        StackObjectsCount = 999999,
                        UnlimitedCount = true,
                    },
                },
            };
        }
        return customAssorts;
    }
    public List<Item> FixAssort(List<Item> assorts)
    {
        var newAssorts = new List<Item>(assorts);
        foreach(var item in newAssorts)
        {
            var oldId = item.Id;
            item.Id = mGUtils.Generate();
            newAssorts = mGUtils.ReplaceKey<List<Item>>(newAssorts, oldId, item.Id);
        }
        return newAssorts;
    }

    private void Log(string data, Color textColor)
    {
        mGUtils.Log("独立预设",data,textColor);
    }
}
