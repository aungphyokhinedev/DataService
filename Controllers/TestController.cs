using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
using Newtonsoft.Json;

namespace DataService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    IRequestClient<DataServiceContract> _client;

    private readonly ILogger<TestController> _logger;
    private IDataContext _db;
    public TestController(ILogger<TestController> logger, IDataContext db, IRequestClient<DataServiceContract> client)
    {
        _client = client;
        _logger = logger;
        _db = db; //http://localhost:5033/AddContext
    }

    [HttpPost]
    public async Task<DataService.Response> Add()
    {
        var newuser = new
        {
            uid = 4,
            nrc = "12/pzt(N)9400",
            pin = "2000033",
            mobile_no = "090400440",
            email = "thiargy@abank.com.mm",
            createdat = (DateTimeOffset)DateTime.Now,
            editedat = (DateTimeOffset)DateTime.Now,
            deleteflag = false
        };
        var contract = new Query("users").Insert(newuser).Contract();

        //direct db calling
        var dbresult = await new Query("users").Insert(newuser).ExecuteAsync(_db);
        //direct test 
        /*   var result = await _db.AddAsync(new CreateRequest{
               table ="users",
               data = parameters
           });
           */

        var result = await contract.ExecuteAsync(_client);
        // var result = await _client.GetResponse<ResultData>(contract);
        var data = result.Data().toList<users>();
        return (Response)result.Message.response;

    }

    [HttpPost]
    public async Task<object> List(int page, int pageSize)
    {
        var paras = new
        {
            uid = 7
        };
        var contract = new Query("users").Select("id,uid,nrc,mobile_no,deviceid").Where("id = users.id", paras).Order("id").Limit(10).Page(1).Contract();

        var result = await contract.ExecuteListAsync(_client);
        
        //var result = await _db.GetListAsync(data);
        var data = result.Data().toList<users>();
        return result.Response();
    }

    [HttpPost]
    public async Task<DataService.Response> Update()
    {

        var edituser = new users
        {
            UID = 250,
            NRC = "12/pzt(N)940045"
        };

        var contract = new Query("users").Update(edituser).Where("id=@id", new { id = 15 }).Contract();
        var result = await contract.ExecuteAsync(_client);


        return result.Response();

    }

    [HttpPost]
    public async Task<DataService.Response> Remove(int id = 10)
    {
        var contract = new Query("users").Delete().Where("id = @nid", new { nid = id }).Contract();
        var result = await contract.ExecuteAsync(_client);
        return result.Response();

    }

    [HttpPost]
    public async Task<DataService.Response> Transaction()
    {

        List<dynamic> arr = new List<dynamic>();

        var r1 = new Query("users")
        .Update(new { uid = 72, deviceid = "123" })
        .Where("id = @id", new { id = 2, })
        .As("u1").Request();

        var r2 = new Query("users").Update(new { deviceid = "@u1.deviceid" }).Where("id = @userid", new { userid = 7 }).Request();

        arr.Add(r1);
        arr.Add(r2);
        var contract = arr.toContract();


        //direct test 
        /*   var result = await _db.AddAsync(new CreateRequest{
               table ="users",
               data = parameters
           });
           */
        var result = await _client.GetResponse<ResultData>(contract);
        return (Response)result.Message.response;

    }

    

}


