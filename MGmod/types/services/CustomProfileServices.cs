using _MGMod.types.models.EFT.templetes;
using _MGMod.types.models.Paths;
using _MGMod.types.utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using _MGMod.types.server;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Logging;
using Path = System.IO.Path;

namespace _MGMod.types.services;
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]

public class CustomProfileServices
{
	private ISptLogger<CustomProfileServices> logger;
	private DatabaseService databaseService;
	private TemplatesServer templatesServer;
	private MGUtils mGUtils;

	public CustomProfileServices(
		ISptLogger<CustomProfileServices> _logger,
		DatabaseService _databaseService,
		TemplatesServer _templatesServer,
		MGUtils _mGUtils
		)
	{
		logger = _logger;
		databaseService = _databaseService;
		templatesServer = _templatesServer;
		mGUtils = _mGUtils;
	}

	public void Start()
	{
		return;
		List<MGProfile> MGProfiles = mGUtils.GetJsonDataFromFile<List<MGProfile>>(Paths.ProfileJson);
		mGUtils.TestOutput(MGProfiles);
		AddProfileToServer(MGProfiles);
		AddProfileToDB(MGProfiles);
        Log("已开启。", LogTextColor.Yellow);
	}

	private void AddProfileToServer(List<MGProfile> mgProfiles)
	{
		String serverPath = "..\\..\\..\\SPT_Data\\database\\locales\\server";
		List<String> serverFiles = mGUtils.GetFiles(serverPath);
		foreach (var serverFile in serverFiles)
		{
			if (!mGUtils.FileExists(serverFile)) continue;
			mGUtils.TestOutput(serverFile);
			var fileName = mGUtils.StripExtension(serverFile);
			var serverTypePath = new PathType
			{
				FileName = $"{fileName}.json",
				Path = serverPath
			};
			Dictionary<string, string> server = mGUtils.GetJsonDataFromFile<Dictionary<string, string>>(serverTypePath);
			foreach (var mgProfile in mgProfiles)
			{
				server.TryAdd(mgProfile.profileSides.DescriptionLocaleKey, mgProfile.description);
			}
			mGUtils.WriteFile(serverFile, mGUtils.Serialize(server));
		}
	}

	private void AddProfileToDB(List<MGProfile> mgProfiles)
	{
		List<WeaponBuild> GunSmith = mGUtils.GetJsonDataFromFile<List<WeaponBuild>>(Paths.GunSmithJson);
		foreach (var mgProfile in mgProfiles)
		{
			mGUtils.TestOutput(mgProfile.profileName);
			mgProfile.profileSides.Bear.UserBuilds.WeaponBuilds.AddRange(GunSmith);
			mgProfile.profileSides.Usec.UserBuilds.WeaponBuilds.AddRange(GunSmith);
			templatesServer.AddProfile(mgProfile);
		}
	}

	private void Log(string data, LogTextColor textColor)
	{
		mGUtils.Log("独立存档", data, textColor);
	}
}
