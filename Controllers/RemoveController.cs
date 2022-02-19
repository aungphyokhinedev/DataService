using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RemoveController : ControllerBase
{
    IRequestClient<RemoveData> _client;

    private readonly ILogger<RemoveController> _logger;
    private IDataContext _db;
    public RemoveController(ILogger<RemoveController> logger, IDataContext db, IRequestClient<RemoveData> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

    [HttpPost]
    public async Task<DataService.Response> Remove()
    {
        var request = new Query("users").Delete().Where("id = @nid", new { nid = 10 }).Request();

        var result = await _client.GetResponse<ResultData>(new { request = request });

        return result.Message.response;

    }


}


