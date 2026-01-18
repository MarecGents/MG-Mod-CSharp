using SPTarkov.Server.Core.Models.Common;

namespace _MGMod.types.models.EFT.templetes;

public class KeyMapType
{
    public HashSet<MongoId> MapBigmap { get; set; }
    public HashSet<MongoId> MapShoreline { get; set; }
    public HashSet<MongoId> MapRezervbase { get; set; }
    public HashSet<MongoId> MapFactory4 { get; set; }
    public HashSet<MongoId> MapWoods { get; set; }
    public HashSet<MongoId> MapLighthouse { get; set; }
    public HashSet<MongoId> MapInterchange { get; set; }
    public HashSet<MongoId> MapLaboratory { get; set; }
    public HashSet<MongoId> MapTarkovstreets { get; set; }
    public HashSet<MongoId> MapSandbox { get; set; }
    public HashSet<MongoId> MapLabyrinth { get; set; }
    public HashSet<MongoId> MapUnknown { get; set; }
}