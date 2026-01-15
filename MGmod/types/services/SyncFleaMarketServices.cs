using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Reflection;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;

using _MGMod.types.models.Paths;
using _MGMod.types.models.EFTofMG.templetes;
using SPTarkov.Server.Core.Models.Utils;
namespace _MGMod.types.services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class SyncFleaMarketServices
{

    private PriceType? priceJson;
    private GitHubTokenType? githubToken;
    private ISptLogger<SyncFleaMarketServices> logger;
    private ModHelper modHelper;
    private DatabaseService databaseService;
    private JsonUtil jsonUtil;
    private FileUtil fileUtil;
    private string ModPath;
    public SyncFleaMarketServices(
        ISptLogger<SyncFleaMarketServices> _logger,
        ModHelper _modHelper,
        DatabaseService _databaseService,
        JsonUtil _jsonUtil,
        FileUtil _fileUtil
        )
    {
        logger = _logger;
        modHelper = _modHelper;
        databaseService = _databaseService;
        jsonUtil = _jsonUtil;
        fileUtil = _fileUtil;
        ModPath = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        Init();
    }

    public async void Start()
    {
        DateTime date = new DateTime(priceJson.date[0], priceJson.date[1], priceJson.date[2]);
        TimeSpan diff = DateTime.Now - date;
        if (diff.TotalDays < 3) return;
        Log("同步数据与当前日期差距过大，正在重新同步。", LogTextColor.Blue);
        await GetPrices();
        LoadPrice();
    }
    private void Init()
    {

        if (fileUtil.FileExists(Path.Combine(Path.Combine(ModPath, Paths.PriceJson.Path), Paths.PriceJson.FileName)))
        {
            priceJson = modHelper.GetJsonDataFromFile<PriceType>(Path.Combine(ModPath, Paths.PriceJson.Path), Paths.PriceJson.FileName);
        }
        else {
            DateTime date = (DateTime.Now).AddDays(-4);
            priceJson = new() { date = [1970, 1, 1], prices = databaseService.GetPrices() };
        }

        if (fileUtil.FileExists(Path.Combine(Path.Combine(ModPath, Paths.GithubToken.Path), Paths.GithubToken.FileName)))
        {
            githubToken = modHelper.GetJsonDataFromFile<GitHubTokenType>(Path.Combine(ModPath, Paths.GithubToken.Path), Paths.GithubToken.FileName);
        }
    }
    private async Task GetPrices()
    {
        if (githubToken == null) return;

        var client = new HttpClient();

        // GitHub API 要求带有 User-Agent
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", githubToken.token);

        string url = $"https://api.github.com/repos/{githubToken.owner}/{githubToken.repo}/contents/{githubToken.filePath}";

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // GitHub 的 contents API 返回的是 Base64 编码的内容
            var jsonDoc = JsonDocument.Parse(content);
            var encodedContent = jsonDoc.RootElement.GetProperty("content").GetString();
            var decodedBytes = Convert.FromBase64String(encodedContent.Replace("\n", ""));
            string fileContent = Encoding.UTF8.GetString(decodedBytes);

            priceJson = jsonUtil.Deserialize<PriceType>(fileContent);
            await SavePrice();
        }
        catch (Exception ex)
        {
            Log($"获取出错<{ex.Message}>", LogTextColor.Red);
        }

    }
    private async Task SavePrice()
    {
        if (priceJson == null) return;

        await fileUtil.WriteFileAsync(Path.Combine(Path.Combine(ModPath, Paths.PriceJson.Path), Paths.PriceJson.FileName), jsonUtil.Serialize(priceJson));

    }

    private void LoadPrice()
    { 
        if (priceJson == null) return;
        var Prices = databaseService.GetTemplates().Prices;
        Prices = priceJson.prices;
		Log($"已同步至日期[{priceJson.date[0]}年{priceJson.date[1]}月{priceJson.date[2]}日]", LogTextColor.Cyan);
    }

    private void Log(string data, LogTextColor textColor)
    {
        logger.LogWithColor("[MG-Mod][实时跳蚤]："+ data, textColor);
    }
}

public class GitHubTokenType
{
    public required string token { get; set; }
    public required string owner { get; set; }
    public required string repo { get; set; }
    public required string filePath { get; set; }
}
