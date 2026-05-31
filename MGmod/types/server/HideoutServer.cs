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

    private void SetConstructionTime(double value)
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
    private void SetProductionTime(double value)
    {
        foreach(var product in Hideout.Production.Recipes)
        {
            if(product.ProductionTime != 0)
            {
                product.ProductionTime = value;
            }
        }
    }
    private void SetScavecaseTime(double value)
    {
        foreach(var scavcase in Hideout.Production.ScavRecipes)
        {
            if(scavcase.ProductionTime != 0)
            {
                scavcase.ProductionTime = value;
            }
        }
    }

    private void SetUpgradeNoLimit()
    {
        foreach(var area in Hideout.Areas)
        {
            foreach(var n in area.Stages.Keys)
            {
                area.Stages[n].Requirements = [];
            }
        }
    }

    private void SetBonusesLevel(int value)
    {
        List<String> RealValue = [
            "EnergyRegeneration", 
            "HydrationRegeneration",
            "HealthRegeneration",
            "MaximumEnergyReserve",
            "StashSize",
        ];
        List<String> AddPercent = [
            "DebuffEndDelay",
            "ExperienceRate",
            "SkillGroupLevelingBoost",
            "QuestMoneyReward",
            "RepairWeaponBonus",
            "RepairArmorBonus",
        ];
        List<String> ReducePercent = [
            "FuelConsumption",
            "ScavCooldownTimer",
            "InsuranceReturnTime",
            "RagfairCommission",
        ];
        List<String> PassItem = [
            "AdditionalSlots",
            "UnlockWeaponModification",
            "UnlockWeaponRepair",
            "UnlockArmorRepair",
            "TextBonus",
        ];

        List<double> TimesValue = [1, 2, 5, 10];
        List<double> AddorReducePercent = [0, 10, 20, 50];
        
        foreach(var area in Hideout.Areas)
        {
            foreach(var n in area.Stages.Keys)
            {
                foreach (var bonus in area.Stages[n].Bonuses)
                {
                    if (RealValue.Contains(bonus.Type.ToString()))
                    {
                        bonus.Value *= TimesValue[value];
                    }
                    else if (AddPercent.Contains(bonus.Type.ToString()))
                    {
                        bonus.Value += AddorReducePercent[value];
                    }
                    else if (ReducePercent.Contains(bonus.Type.ToString()))
                    {
                        bonus.Value -= AddorReducePercent[value];
                    }
                    else if (PassItem.Contains(bonus.Type.ToString()))
                    {
                        continue;
                    }
                }
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
        // 功能：藏身处升级无限制 UpgradeNoLimit
        if (HideoutSetting.UpgradeNoLimit)
        {
            SetUpgradeNoLimit();
        }
        
        // 功能：藏身处区域加成等级 BonusesLevel
        if (HideoutSetting.BonusesLevel.enable)
        {
            SetBonusesLevel(HideoutSetting.BonusesLevel.value);
        }
    }
}
