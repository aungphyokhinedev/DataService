using MassTransit;
using AplusExtension;
namespace DataService;

public class AddDataConsumer : IConsumer<AddData> 
{
    private IDataContext _db;
    public AddDataConsumer( IDataContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<AddData> context)
    {   
        
        var result = await _db.AddAsync(RequestTransformer.toInsert(context.Message.request));
       
        await context.RespondAsync<ResultData>(new
        {
            response = result
        });
    }

   
}

