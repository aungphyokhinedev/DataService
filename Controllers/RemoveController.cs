using Microsoft.AspNetCore.Mvc;
using AplusDbContext;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]")]
public class RemoveController : ControllerBase
{
    IRequestClient<AddData> _client;

    private readonly ILogger<RemoveController> _logger;
    private IDataContext _db;
    public RemoveController(ILogger<RemoveController> logger, IDataContext db,IRequestClient<AddData> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

        //http://localhost:5033/AddContext
        public async Task<AplusDbContext.Response> Update()
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
             var result = await _db.RemoveAsync(new RemoveRequest{
                 table ="users",
                 filter = new Filter{
                    where = "id = @nid",
                    parameters = new Dictionary<string, object>{
                        {"nid" , 5 }
                    }.toParameterList()
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


