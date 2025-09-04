using _MGMod.types.models.Paths;
using _MGMod.types.utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Utils;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class CustomTraderServices(
    MGUtils mgUtils
    )
{
    public void start()
    {
        var TradersDirectories = mgUtils.GetDirectories(Paths.Traders);
    }

    public void AddTraderToDB()
    {

    }

    public void AddImageToDB()
    {

    }

    public void AddTraderDataToDB()
    {

    }

    public void AddTraderItemsToDB()
    {

    }

    public void AddTraderLocalesToDB()
    {

    }

    public void AddTraderLocationToDB()
    {

    }

    public void AddTraderTemplatesToDB()
    {

    }

    public void AddTraderBundlesToDB()
    {

    }

    public void AddTraderGlobalsToDB()
    {

    }

}