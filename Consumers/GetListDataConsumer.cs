using MassTransit;
namespace DataService;

public class GetListDataConsumer : IConsumer<GetList> 
{
    private IDataContext _db;
    public GetListDataConsumer( IDataContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<GetList> context)
    {   
        
        var result = await _db.GetListAsync(context.Message.request);
       
        await context.RespondAsync<ListData>(new
        {
            response = result
        });
    }
}

