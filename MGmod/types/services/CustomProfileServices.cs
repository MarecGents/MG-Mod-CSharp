using _MGMod.types.models.EFT.templetes;
using _MGMod.types.models.Paths;
using _MGMod.types.utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using _MGMod.types.server;
using SPTarkov.Common.Logger;
using SPTarkov.Server.Core.Models.Eft.Profile;
using Color = Spectre.Console.Color;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]

public class CustomProfileServices(
	SptLogger<CustomProfileServices> logger,
	TemplatesServer templatesServer,
	MGUtils mGUtils
	)
{

	public void Start()
	{
		List<MGProfile> MGProfiles = mGUtils.GetJsonDataFromFile<List<MGProfile>>(Paths.ProfileJson);
		AddProfileToServer(MGProfiles);
		AddProfileToDB(MGProfiles);
        Log("已开启。", Color.Yellow);
		return;
	}

	private void AddProfileToServer(List<MGProfile> mgProfiles)
	{
		String serverPath = "..\\..\\..\\SPT_Data\\database\\locales\\server";
		List<String> serverFiles = mGUtils.GetFiles(serverPath);
		foreach (var serverFile in serverFiles)
		{
			if (!mGUtils.FileExists(serverFile, false)) continue;
			var fileName = mGUtils.StripExtension(serverFile);
			var serverTypePath = new PathType
			{
				FileName = $"{fileName}.json",
				Path = serverPath
			};
			Dictionary<string, string> server = mGUtils.GetJsonDataFromFile<Dictionary<string, string>>(serverTypePath);
			int flag = 0;
			foreach (var mgProfile in mgProfiles)
			{
				bool v = server.TryAdd(mgProfile.profileSides.DescriptionLocaleKey, mgProfile.description);
				if (v) flag += 1;
			}
			if (flag == 0) continue;
			mGUtils.DeleteFile(serverFile, false);
			mGUtils.WriteFile(serverFile, mGUtils.Serialize(server),false);
		}
	}

	private void AddProfileToDB(List<MGProfile> mgProfiles)
	{
		List<WeaponBuild> GunSmith = mGUtils.GetJsonDataFromFile<List<WeaponBuild>>(Paths.GunSmithJson);
		foreach (var mgProfile in mgProfiles)
		{
			Dictionary<string, WeaponBuild> weaponBuilds = new Dictionary<string, WeaponBuild>();
			foreach (var gunSmith in GunSmith)
			{
				weaponBuilds.TryAdd(gunSmith.Name, gunSmith);
			}
			mgProfile.profileSides.Bear.WeaponBuilds = weaponBuilds;
			mgProfile.profileSides.Usec.WeaponBuilds = weaponBuilds;

			mgProfile.profileSides.Bear.UserBuilds = new UserBuilds();
			mgProfile.profileSides.Bear.UserBuilds.WeaponBuilds = new List<WeaponBuild>();
			mgProfile.profileSides.Bear.UserBuilds.WeaponBuilds.AddRange(GunSmith);
			mgProfile.profileSides.Usec.UserBuilds = new UserBuilds();
			mgProfile.profileSides.Usec.UserBuilds.WeaponBuilds = new List<WeaponBuild>();
			mgProfile.profileSides.Usec.UserBuilds.WeaponBuilds.AddRange(GunSmith);
			templatesServer.AddProfile(mgProfile);
		}
	}

	private void Log(string data, Color textColor)
	{
		mGUtils.Log("独立存档", data, textColor);
	}
}
