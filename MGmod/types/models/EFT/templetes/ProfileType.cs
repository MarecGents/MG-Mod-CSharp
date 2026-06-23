using SPTarkov.Server.Core.Models.Eft.Common.Tables;

namespace _MGMod.types.models.EFT.templetes;

public class MGProfile
{
	public required string profileName { get; set; }
	public required ProfileSides profileSides { get; set; }
	public string? description { get; set; }
}