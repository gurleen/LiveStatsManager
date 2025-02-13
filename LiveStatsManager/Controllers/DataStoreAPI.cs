using LiveStatsManager.FileWatcher;
using LiveStatsManager.Services.DataStore;
using Microsoft.AspNetCore.Mvc;

namespace LiveStatsManager.Controllers;

[ApiController]
public class DataStoreAPI(IDataStore dataStore) : Controller
{
    [HttpGet("/dataStore")]
    public IActionResult Index()
    {
        return Json(dataStore.GetAll());
    }
    
    [HttpPost("/dataStore/update")]
    public IActionResult Update([FromBody]DataPair dataPair)
    {
        dataStore.Add(dataPair);
        return Ok();
    }
    
    [HttpPost("/dataStore/toggle/{key}")]
    public IActionResult Toggle(string key)
    {
        var rand = Random.Shared.NextInt64().ToString();
        var dataPair = new DataPair(key, rand);
        dataStore.Add(dataPair);
        return Ok();
    }
}