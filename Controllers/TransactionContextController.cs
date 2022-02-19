using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
using Newtonsoft.Json;

namespace DataService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TransactionController : ControllerBase
{
    IRequestClient<TransactionData> _client;

    private readonly ILogger<TransactionController> _logger;
    private IDataContext _db;
    public TransactionController(ILogger<TransactionController> logger, IDataContext db,IRequestClient<TransactionData> client)
    {
        _client = client;
        _logger = logger;
        _db = db; 
    }

       [HttpPost]
        public async Task<DataService.Response> Do()
        {
         
            List<dynamic> arr = new List<dynamic>();

            var r1 = new Query("users")
            .Update(new{uid = 72,deviceid = "789"})
            .Where("id = @id",new { id= 6, })
            .As("u1").Request();

            var r2 = new Query("users").Update(new{deviceid = "@u1.deviceid"}).Where("id = @userid",new{userid = 7}).Request();
             
            arr.Add(r1);
            arr.Add(r2);
            var requests = arr.toTypedQueryList();


            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            var result = await _client.GetResponse<ResultData>(new {requests = requests});
            return   (Response)result.Message.response;
        
        }

        
}


