using SPTarkov.Server.Core.Models.Eft.Common;

namespace _MGMod.types.models.EFT.locations;

public class CustomLooseLoot
{
    public SpawnpointCount? spawnpointCount { get; set; }
    public List<Spawnpoint>? spawnpointsForced { get; set; }
    public List<Spawnpoint>? spawnpoints { get; set; }
}
