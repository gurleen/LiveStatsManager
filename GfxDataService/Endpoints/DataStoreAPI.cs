using GfxDataService.DataStore;
using GfxDataService.FileWatcher;
using Microsoft.AspNetCore.Mvc;

namespace GfxDataService.Endpoints;

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