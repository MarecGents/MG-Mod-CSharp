using _MGMod.types.models.EFT.traders;
using _MGMod.types.models.Paths;
using _MGMod.types.utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Loaders;
using SPTarkov.Server.Core.Utils;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class CustomTraderServices
{
    private MGUtils mGUtils;
    public CustomTraderServices(
        MGUtils _mGUtils
    )
    {
        mGUtils = _mGUtils;
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
            
        }
        return Bundles;
    }

    public void AddBundles(BundleManifest Bundles)
    {
        
    }

    public void AddTraderBaseToDB(string traderPath)
    {
        CustomTraderInfo traderInfo = mGUtils.GetJsonDataFromFile<CustomTraderInfo>(new PathType
        {
            FileName = TraderPathsType.TraderInfo,
            Path = traderPath,
            
        });
        if (traderInfo == default)
        {
            // 2026.01.19 23:14 进度于此
        }
        
    }

    public void AddImageToDB(string traderPath)
    {
        
    }

    public void AddTraderDataToDB(string traderPath)
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