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
        _db = db; //http://localhost:5033/AddContext
    }

       [HttpPost]
        public async Task<DataService.Response> Do()
        {
         
            List<QueryContext> requests = new List<QueryContext>();
             
            requests.Add(new UpdateContext{
                tag = "update1",
                 table ="users",
                 data = new Dictionary<string, object>{
                        {"UID" , 70 }
                    },
                where = "id = @nid",
                    whereParams = new Dictionary<string, object>{
                        {"nid" , 6 }
                    }
                
             });

             requests.Add(new UpdateContext{
                 table ="users",
                 data = new Dictionary<string, object>{
                        {"UID" , 71 }
                    },
                extraValues = new []{new ExtraValue{
                    tag="update1",
                    fieldName="id",
                    parameterName="uid"
                }}.ToList(),
                 where = "id = @nid",
                    whereParams = new Dictionary<string, object>{
                        {"nid" , 7 }
                    }
                
             });


            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            var result = await _client.GetResponse<ResultData>(new {json = JsonConvert.SerializeObject(requests.toQueryRequest())});

           
           
            return   (Response)result.Message.response;
        
        }

        
}


