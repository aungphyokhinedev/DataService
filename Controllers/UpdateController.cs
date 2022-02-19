using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UpdateController : ControllerBase
{
    IRequestClient<UpdateData> _client;

    private readonly ILogger<UpdateController> _logger;
    private IDataContext _db;
    public UpdateController(ILogger<UpdateController> logger, IDataContext db,IRequestClient<UpdateData> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

       [HttpPost]
        public async Task<DataService.Response> Update()
        {
            
            var edituser = new users{
                UID = 250,
                NRC = "12/pzt(N)940045"
            };

            var request = new Query("users").Update(edituser).Where("id=@id",new {id=15}).Request();
             
            var result = await _client.GetResponse<ResultData>(new {request = request});
          
            return result.Message.response;
        
        }

        
}


