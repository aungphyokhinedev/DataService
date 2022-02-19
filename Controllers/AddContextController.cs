using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AddController : ControllerBase
{
    IRequestClient<AddData> _client;

    private readonly ILogger<AddController> _logger;
    private IDataContext _db;
    public AddController(ILogger<AddController> logger, IDataContext db,IRequestClient<AddData> client)
    {
        _client = client;
        _logger = logger;
        _db = db; //http://localhost:5033/AddContext
    }

       [HttpPost]
        public async Task<DataService.Response> Add()
        {
            var newuser = new {
                uid = 4,
                nrc = "12/pzt(N)9400",
                pin = "2000033",
                mobile_no = "090400440",
                email = "thiargy@abank.com.mm",
                createdat = (DateTimeOffset)DateTime.Now,
                editedat = (DateTimeOffset)DateTime.Now,
                deleteflag = false
            };
            var request = new Query("users").Insert(newuser).Request();

            //direct db calling
           var dbresult = await new Query("users").Insert(newuser).ExecuteAsync(_db);
            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            var result = await _client.GetResponse<ResultData>(new {request = request});
            var data = result.Message.response.rows.toList<users>();
            return   (Response)result.Message.response;
        
        }

        
}


