using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ListController : ControllerBase
{
    IRequestClient<GetList> _client;

    private readonly ILogger<ListController> _logger;
    private IDataContext _db;
    public ListController(ILogger<ListController> logger, IDataContext db,IRequestClient<GetList> client)
    {
        _client = client;
        _logger = logger;
        _db = db;
    }

   [HttpPost]
        public async Task<object> GetList(int page, int pageSize)
        {   
            var finduser = new {
                uid = 6
            };
            var request = new Query("users").Where("id = @uid").Set(finduser).Select("id,uid,nrc,mobile_no").Order("id").Limit(10).Page(1).Request();
            
           
           var result = await _client.GetResponse<ListData>(new {request=request});

           //var result = await _db.GetListAsync(data);
            var data =  result.Message.response.rows.toList<users>();
            return  result.Message.response;
        }


}
