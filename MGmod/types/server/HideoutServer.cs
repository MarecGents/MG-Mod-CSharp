using _MGMod.types.models.Custom;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Hideout;
using SPTarkov.Server.Core.Models.Spt.Hideout;
using SPTarkov.Server.Core.Services;

namespace _MGMod.types.server;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class HideoutServer(
    DatabaseService databaseService
    )
{
    private Hideout Hideout => databaseService.GetHideout();

    public Hideout GetHideout()
    {
        return Hideout;
    }

    public void SetConstructionTime(double value)
    {
        foreach(var area in Hideout.Areas)
        {
            foreach(var n in area.Stages.Keys)
            {
                if(area.Stages[n].ConstructionTime != 0)
                {
                    area.Stages[n].ConstructionTime = value;
                }
            }
        }
    }
    public void SetProductionTime(double value)
    {
        foreach(var product in Hideout.Production.Recipes)
        {
            if(product.ProductionTime != 0)
            {
                product.ProductionTime = value;
            }
        }
    }
    public void SetScavecaseTime(double value)
    {
        foreach(var scavcase in Hideout.Production.ScavRecipes)
        {
            if(scavcase.ProductionTime != 0)
            {
                scavcase.ProductionTime = value;
            }
        }
    }
    public void MGmodHideout(MGModConfig_Hideout HideoutSetting)
    {
        // 功能：藏身处升级时间 BuildTime
        if (HideoutSetting.BuildTime.enable)
        {
            SetConstructionTime(HideoutSetting.BuildTime.value);
        }
        // 功能：藏身处生产时间 ProductTime
        if (HideoutSetting.ProductTime.enable)
        {
            SetProductionTime(HideoutSetting.ProductTime.value);
        }
        // 功能：Scav宝箱
        if (HideoutSetting.ScavCaseTime.enable)
        {
            SetScavecaseTime(HideoutSetting.ScavCaseTime.value);
        }
    }
}
