using MassTransit;
namespace DataService;

public class UpdateDataConsumer : IConsumer<UpdateData> 
{
    private IDataContext _db;
    public UpdateDataConsumer( IDataContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<UpdateData> context)
    {   
        
        var result = await _db.UpdateAsync(context.Message.request);
       
        await context.RespondAsync<ResultData>(new
        {
            response = result
        });
    }
}

