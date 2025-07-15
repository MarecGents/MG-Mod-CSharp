using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Helpers;
using System.Reflection;
using _MGMod.types.services;
using SPTarkov.Server.Core.Utils;
using _MGMod.types.models.Custom;
using _MGMod.types.server;

namespace _MGMod;

public record ModMetadata : AbstractModMetadata
{
    public override string Name { get; set; } = "MGMod";
    public override string Author { get; set; } = "MarecGents";
    public override List<string>? Contributors { get; set; }
    public override string Version { get; set; } = "0.8.0";
    public override string SptVersion { get; set; } = "4.0.0";
    public override List<string>? LoadBefore { get; set; }
    public override List<string>? LoadAfter { get; set; }
    public override List<string>? Incompatibilities { get; set; }
    public override Dictionary<string, string>? ModDependencies { get; set; }
    public override string? Url { get; set; }
    public override bool? IsBundleMod { get; set; } = true;
    public override string? Licence { get; set; } = "MIT license";
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class MGmod(
    ISptLogger<MGmod> logger,
    ModHelper modHelper,
    ConfigSettingServices configSettingServices
    ) : IOnLoad
{
    public Task OnLoad()
    {
        var modPath = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        logger.LogWithColor("This is MGmod", LogTextColor.Red);
        configSettingServices.ModSetting();
        return Task.CompletedTask;
    }
}

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 1)]
public class PreMGmodLoad(
    ISptLogger<PreMGmodLoad> logger
    ) : IOnLoad
{
    public Task OnLoad()
    {
        logger.LogWithColor("This is PreMGmodLoad", LogTextColor.Red, LogBackgroundColor.Cyan);
        return Task.CompletedTask;
    }
}