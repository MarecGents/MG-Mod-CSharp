using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using _MGMod.types.services;
using _MGMod.types.utils;
using SPTarkov.Common.Logger;
using SPTarkov.Server.Core.Helpers.Server;

namespace _MGMod;

public record ModMetadata : IModMetadata
{
	public string ModGuid { get; init; } = "com.marecgents.tarkovmod.mgmod";
	public string Name { get; init; } = "MGMod";
	public string Author { get; init; } = "MarecGents";
    public List<string>? Contributors { get; init; } = ["MarecGents"];
	public SemanticVersioning.Version Version { get; init; } = new("1.0.0");
	public SemanticVersioning.Range SptVersion { get; init; } = new("4.1.2");
    public bool HasPrepatcher { get; init; } = false;
	public List<string>? Incompatibilities { get; init; }
	public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; } = "https://github.com/MarecGents/MG-Mod/releases/latest";
	public string License { get; init; } = "CC BY-NC-ND 4.0";
}

[Injectable(TypePriority = OnLoadOrder.Preload + 1)]
public class MGmod(
    SptLogger<MGmod> logger,
    ModHelper modHelper,
    ConfigSettingServices configSettingServices,
    MGUtils  mGUtils
    ) : IOnLoad
{
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        // var modPath = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        // logger.LogWithColor("This is MGmod", LogTextColor.Red);
        await configSettingServices.ModSetting();
    }
}

// [Injectable(TypePriority = OnLoadOrder.Preload + 1)]
// public class PreMGmodLoad(
//     SptLogger<PreMGmodLoad> logger
//     ) : IOnLoad
// {
//     public Task OnLoadAsync(CancellationToken cancellationToken)
//     {
//         // logger.LogWithColor("This is PreMGmodLoad", zLogTextColor.Red, LogBackgroundColor.Cyan);
//         return Task.CompletedTask;
//     }
// }