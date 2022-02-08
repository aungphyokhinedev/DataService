using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using AplusDbContext;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]")]
public class AddController : ControllerBase
{
    IRequestClient<AddData> _client;

    private readonly ILogger<AddController> _logger;
    private IDataContext _db;
    public AddController(ILogger<AddController> logger, IDataContext db,IRequestClient<AddData> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

        //http://localhost:5033/AddContext
        public async Task<AplusDbContext.Response> Add()
        {
         
            var data = new Dictionary<string, object>{
                        {"UID" , 4 },
                        {"NRC" , "12/pzt(N)9400" },
                        {"PIN" , "12120" },
                        {"Mobile_no" , "059505" },
                        {"Email" , "old@gmail.com" },
                        {"CreatedAt" , (DateTimeOffset)DateTime.Now },
                        {"EditedAt" , (DateTimeOffset)DateTime.Now },
                        {"DeleteFlag" , false },
                    };
           
            var parameters = data.toParameterList();

            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            var result = await _client.GetResponse<ResultData>(new {request = new CreateRequest {
                table = "users",
                data = parameters
            }});

           
           
            return   result.Message.response;
        
        }

        
}


