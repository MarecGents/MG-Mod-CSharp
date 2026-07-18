using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;

using _MGMod.types.models.Paths;
using _MGMod.types.models.EFT.templetes;
using _MGMod.types.server;
using _MGMod.types.utils;
using SPTarkov.Server.Core.Models.Utils;
namespace _MGMod.types.services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class SyncFleaMarketServices
{

    private PriceType? priceJson;
    private MGUtils mGUtils;
    private DatabaseService databaseService;
    private ConfigsServer  configsServer;
    
    public SyncFleaMarketServices(
        DatabaseService _databaseService,
        ConfigsServer _configsServer,
        MGUtils _mGUtils
        )
    {
        databaseService = _databaseService;
        configsServer = _configsServer;
        mGUtils = _mGUtils;
    }

    public async Task Start()
    {
        configsServer.ApplyBaseFleaPrices();
        await Init();
    }

    public async Task Init()
    {
        if (!mGUtils.FileExists(Path.Combine(Paths.PriceJson.Path, Paths.PriceJson.FileName)))
        {
            DateTime date = (DateTime.Now).AddDays(-4);
            priceJson = new PriceType { date = [date.Year, date.Month, date.Day], prices = databaseService.GetPrices() };
        }
        else
        {
            priceJson = mGUtils.GetJsonDataFromFile<PriceType>(Paths.PriceJson);
        }

        if (priceJson == null) return;

        DateTime nowDate = new DateTime(priceJson.date[0], priceJson.date[1], priceJson.date[2]);
        TimeSpan diff = DateTime.Now - nowDate;
        if (diff.TotalDays < 3)
        {
            LoadPrice();
        }
        else
        {
            Log("同步数据与当前日期差距过大，正在重新同步。", LogTextColor.Cyan);
            await GetPrices();
            if (priceJson != null) LoadPrice();
        }
    }

    /// <summary>
    /// 多级回退获取价格数据：
    /// ① jsDelivr CDN（中国大陆友好）→ ② raw.githubusercontent.com（官方源）→ ③ 使用本地缓存
    /// </summary>
    private async Task GetPrices()
    {
        string[] urls = GetPriceUrls();

        foreach (var url in urls)
        {
            if (await TryFetchPriceFromUrl(url)) return;
        }

        Log("所有外部源均不可用，已保留本地缓存数据。", LogTextColor.Cyan);
    }

    private string[] GetPriceUrls()
    {
        return
        [
            "https://cdn.jsdelivr.net/gh/MarecGents/MG-FleaMarket@main/res/price.json",
            "https://raw.githubusercontent.com/MarecGents/MG-FleaMarket/main/res/price.json"
        ];
    }

    private async Task<bool> TryFetchPriceFromUrl(string url)
    {
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            string json = await client.GetStringAsync(url);

            var fetched = mGUtils.Deserialize<PriceType>(json);
            if (fetched == null)
            {
                Log($"从 [{url}] 获取数据格式异常。", LogTextColor.Yellow);
                return false;
            }

            priceJson = fetched;
            SavePrice();
            Log($"已从 CDN 同步最新价格数据。", LogTextColor.Green);
            return true;
        }
        catch (Exception ex)
        {
            Log($"从 [{url}] 获取失败: {ex.Message}", LogTextColor.Yellow);
            return false;
        }
    }

    private void SavePrice()
    {
        if (priceJson == null) return;
        mGUtils.WriteFile(Path.Combine(Paths.PriceJson.Path, Paths.PriceJson.FileName), mGUtils.Serialize(priceJson));
    }

    private void LoadPrice()
    { 
        if (priceJson == null) return;
        var prices = databaseService.GetPrices();
        foreach (var id in prices.Keys)
        {
            if (priceJson.prices.TryGetValue(id, out var price))
            {
                prices[id] = price;
            }
        }

        Log($"已同步至日期 {priceJson.date[0]}年{priceJson.date[1]}月{priceJson.date[2]}日。", LogTextColor.Yellow);
    }
    
    private void Log(string data, LogTextColor textColor)
    {
        mGUtils.Log("实时跳蚤", data, textColor);
    }
}
