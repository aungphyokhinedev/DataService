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
    public RemoveController(ILogger<RemoveController> logger, IDataContext db,IRequestClient<RemoveData> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

      [HttpPost]
        public async Task<DataService.Response> Remove()
        {
           
            
            var data = new Dictionary<string, object>{
                        {"UID" , 250 },
                        {"NRC" , "12/pzt(N)9400" },
                        {"PIN" , "12120" },
                        {"Mobile_no" , "12345" },
                      //  {"Email" , "new@gmail.com" },
                      
                        {"DeleteFlag" , true },
                    };
           
            var parameters = data.toParameterList();

            //direct test 
             var result = await _db.RemoveAsync(new DeleteContext{
                 table ="users",
                 where = "id = @nid",
                    whereParams = new Dictionary<string, object>{
                        {"nid" , 5 }
                    }
             });
             
           /* var result = await _client.GetResponse<ResultData>(new {request = new CreateRequest {
                table = "users",
                data = parameters
            }});

           */
           
            return result;  //result.Message.response;
        
        }

        
}


