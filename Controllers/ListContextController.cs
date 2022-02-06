using Microsoft.AspNetCore.Mvc;
using AplusDbContext;
using MassTransit;
namespace DataService.Controllers;

[ApiController]
[Route("[controller]")]
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

    //http://localhost:5033/ListContext?PageSize=15&page=1
        public async Task<object> GetList(int page, int pageSize)
        {
            var request = new GetRequest{
                tables="users", pageSize= pageSize, page= page,
                fields = "id,nrc,mobile_no,createdat",
                orderBy = "id desc",
                /*filter = new Filter{
                    where = "id = 4",
                    parameters = new Dictionary<string, object>{
                        {"id" , 4 }
                    }.toParameterList()
                }*/
            };
           
           var result = await _client.GetResponse<ListData>(new {request=request});

           // var result = await _db.GetListAsync(request);
           
            return  result.Message.response;
        }


}
