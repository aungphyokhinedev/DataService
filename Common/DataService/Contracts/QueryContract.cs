using MassTransit;

namespace  DataService;
public class QueryContract : DataServiceContract
{
    public QueryTypes type { get;set; }
    public string request { get;set; }
    
    public async Task<Response<ListData>> ExecuteListAsync(IRequestClient<DataServiceContract> client){
        if(this.type != QueryTypes.Listing) throw new Exception("Query type does not match");
        return await client.GetResponse<ListData>(this);  
    }

    public async Task<Response<ResultData>> ExecuteAsync(IRequestClient<DataServiceContract> client){
        if(this.type == QueryTypes.Listing) throw new Exception("Query type does not match");
        return await client.GetResponse<ResultData>(this);  
    }


    

}