using Microsoft.AspNetCore.Mvc;

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
            var request = new GetRequest{
                tables="users", pageSize= pageSize, page= page,
                fields = "id,uid,nrc,mobile_no,createdat",
                orderBy = "id",
                /*filter = new Filter{
                    where = "id = 4",
                    parameters = new Dictionary<string, object>{
                        {"id" , 4 }
                    }.toParameterList()
                }*/
            };

            var data = new SelectContext{
                tables="users", pageSize= pageSize, page= page,
                fields = "id,uid,nrc,mobile_no,createdat",
                orderBy = "id",
              /*  where = "id = 6",
                    whereParams = new Dictionary<string, object>{
                        {"id" , 4 }
                    }*/
            };
           
          // var result = await _client.GetResponse<ListData>(new {request=request});

           var result = await _db.GetListAsync(data);
           
            return  result; //result.Message.response;
        }


}
