
using Npgsql;
using DataService;

public class PostgresDataContext : IDataContext
{
    IConfiguration _config;
    
    public PostgresDataContext(IConfiguration cofig){
        _config = cofig;
    }

    public async Task<ListResponse> GetListAsync(SelectContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.getAsync(data,connection);
                
            }
            catch (Exception e)
            {
                return new ListResponse
                {
                    code = ResultCode.InternalServerError,
                    message = e.Message
                };
            }
        }

    }

    public async Task<Response> AddAsync(InsertContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.insertAsync(data,connection);
               
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.InternalServerError,
                    message = e.Message
                };
            }
        }
        
    }

    public async Task<Response> UpdateAsync(UpdateContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.setAsync(data,connection);
                
                
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.InternalServerError,
                    message = e.Message
                };
            }
        }
        
    }

    public async Task<Response> RemoveAsync(DeleteContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.deleteAsync(data,connection);
                
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.InternalServerError,
                    message = e.Message
                };
            }
        }
    }

    public async Task<IResponse> TransactionAsync(List<QueryContext> requests)
    {
         using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                List<IResponse> responses = new List<IResponse>();

                using (var transaction = connection.BeginTransaction())
                {
                    try{

                    foreach(var request in requests){
                        if(request.GetType() == typeof(SelectContext)){
                          var result =  await PostgresFunction.getAsync((SelectContext)request,connection,transaction);
                          responses.Add(result);
                        }
                        if(request.GetType() == typeof(InsertContext)){
                          var result =  await PostgresFunction.insertAsync((InsertContext)request,connection,transaction);
                          responses.Add(result);
                        }
                        if(request.GetType() == typeof(UpdateContext)){
                          var result =  await PostgresFunction.setAsync((UpdateContext)request,connection,transaction);
                          responses.Add(result);
                        }
                        if(request.GetType() == typeof(DeleteContext)){
                          var result =  await PostgresFunction.deleteAsync((DeleteContext)request,connection,transaction);
                          responses.Add(result);
                        }
                    }
                
                    transaction.Commit();
                   
                    
                    }
                    catch(Exception ex){
                        transaction.Rollback();
                        return new Response
                        {
                            code = ResultCode.BadRequest,
                            message = ex.Message
                        };
                    }
                }
                
                return responses.Last();
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.InternalServerError,
                    message = e.Message
                };
            }
        }
    }


   
}