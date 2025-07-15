using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class CustomTraderServices
{
}
