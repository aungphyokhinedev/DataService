
using Dapper;
using Npgsql;
using AplusExtension;
using AplusDbContext;

public class PostgresDataContext : IDataContext
{
    IConfiguration _config;
    
    public PostgresDataContext(IConfiguration cofig){
        _config = cofig;
    }

    public async Task<ListResponse> GetListAsync(GetRequest request)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();

                string query = $"Select {request.fields} from {request.tables}";
                object parameters = new object{};
                //adding where clause 
                //instead of using value directly
                //consider using parameter to prevent SQL injection
                //eg where name = @name and then add @name value in request parameters
               if (request.filter.IsNotNullOrEmpty())
                {
                    query = $"{query} where {request.filter.where}";
                    parameters = request.filter.parameters.toDictionaryList();
                }
     
                //adding group by clause
                if (request.groupBy.IsNotNullOrEmpty())
                {
                    query = $"{query} group by {request.groupBy}";
                }

                //adding order by clause
                if (request.orderBy.IsNotNullOrEmpty())
                {
                    query = $"{query}  order by {request.orderBy}";
                }


                query = $"{query} offset {((request.page - 1) * request.pageSize)} limit {request.pageSize};";



                var value = await connection.QueryAsync(query, parameters);

                ///get total for pagination
                long total = 0;
                query = $"select count(*) as total_rows from  {request.tables}";
               if (request.filter.IsNotNullOrEmpty())
                {
                    query = $"{query} where {request.filter.where};";
                }
                
                var countvalue = await connection.QueryAsync(query, parameters);
                total = countvalue.SingleOrDefault().total_rows;


                return new ListResponse
                {
                    code = ResultCode.Success,
                    total = total,
                    page = request.page,
                    pageSize = request.pageSize,
                    rows = value.Select(x => x as IDictionary<string,object>).ToList()
                };
            }
            catch (Exception e)
            {
                return new ListResponse
                {
                    code = ResultCode.Fail,
                    message = e.Message
                };
            }
        }

    }

    public async Task<Response> AddAsync(CreateRequest request)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                
                string columns = String.Join(",", request.data.Select(x=>x.key));
                string values = String.Join(",", request.data.Select(x=>$"@{x.key}"));
                Dictionary<string,object> parameters = request.data.toDictionaryList();
                
      
                string query = $"INSERT INTO {request.table} ({columns}) VALUES ({values}) RETURNING *;";

                var created = await connection.QueryAsync(query, parameters);
                
                return new Response
                {
                    code = ResultCode.Success,
                    message = "ok",
                    rows = created.Select(x => x as IDictionary<string,object>).ToList()
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.Fail,
                    message = e.Message
                };
            }
        }
        
    }

    public async Task<Response> UpdateAsync(UpdateRequest request)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                
                
                string values = String.Join(",", request.data.Select(x=>$"{x.key} = @{x.key}"));
                Dictionary<string,object> updatedata = request.data.toDictionaryList();
                Dictionary<string,object> wherevalues = request.filter.parameters.toDictionaryList();

                var parameters = updatedata.Concat(wherevalues);
      
                string query = $"UPDATE {request.table} SET {values} WHERE {request.filter.where} RETURNING *;";

                var updated = await connection.QueryAsync(query, parameters);
                
                return new Response
                {
                    code = ResultCode.Success,
                    message = "ok",
                    rows = updated.Select(x => x as IDictionary<string,object>).ToList()
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.Fail,
                    message = e.Message
                };
            }
        }
        
    }

    public async Task<Response> RemoveAsync(RemoveRequest request)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                
                Dictionary<string,object> parameters = request.filter.parameters.toDictionaryList();

                string query = $"DELETE FROM {request.table}  WHERE {request.filter.where} RETURNING *;";

                var deleted = await connection.QueryAsync(query, parameters);
                
                return new Response
                {
                    code = ResultCode.Success,
                    message = "ok",
                    rows = deleted.Select(x => x as IDictionary<string,object>).ToList()
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.Fail,
                    message = e.Message
                };
            }
        }
    }
}