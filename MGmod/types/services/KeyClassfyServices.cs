using _MGMod.types.server;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class KeyClassfyServices
{
    
    private LocalesServer  localesServer;
    
    public KeyClassfyServices(
        LocalesServer _localesServer
        )
    {
        localesServer = _localesServer;
    }

    public void Start()
    {
        HashSet<MongoId> keyCardId = ["5c518ec986f7743b68682ce2", "5c518ed586f774119a772aee"];
        
    }
}