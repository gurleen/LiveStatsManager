using GfxDataService.DataStore;
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
}