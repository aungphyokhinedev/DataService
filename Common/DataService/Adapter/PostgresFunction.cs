using System.Runtime.InteropServices;
using AplusExtension;
using Dapper;
using Npgsql;
namespace DataService;

public class PostgresFunction
{
    public async Task<ListResponse> getAsync(SelectContext data, NpgsqlConnection connection, [Optional] NpgsqlTransaction transaction)
    {
        string query = $"Select {data.fields} from {data.tables}";
        object parameters = new object { };
        //adding where clause 
        //instead of using value directly
        //consider using parameter to prevent SQL injection
        //eg where name = @name and then add @name value in request parameters
        if (data.where.IsNotNullOrEmpty())
        {
            query = $"{query} where {data.where}";
            parameters = data.whereParams;
        }

        //adding group by clause
        if (data.groupBy.IsNotNullOrEmpty())
        {
            query = $"{query} group by {data.groupBy}";
        }

        //adding order by clause
        if (data.orderBy.IsNotNullOrEmpty())
        {
            query = $"{query}  order by {data.orderBy}";
        }


        query = $"{query} offset {((data.page - 1) * data.pageSize)} limit {data.pageSize};";



        var value = transaction.IsNotNullOrEmpty() ? await connection.QueryAsync(query, parameters, transaction) : await connection.QueryAsync(query, parameters);

        ///get total for pagination
        long total = 0;
        query = $"select count(*) as total_rows from  {data.tables}";
        if (data.where.IsNotNullOrEmpty())
        {
            query = $"{query} where {data.where};";
        }

        var countvalue = transaction.IsNotNullOrEmpty() ? await connection.QueryAsync(query, parameters, transaction) : await connection.QueryAsync(query, parameters);
        total = countvalue.SingleOrDefault().total_rows;


        return new ListResponse
        {
            code = StatusCodes.Status200OK,
            total = total,
            page = data.page,
            pageSize = data.pageSize,
            rows = value.Select(x => x as IDictionary<string, object>).ToList()
        };
    }
    public async Task<Response> insertAsync(InsertContext data, NpgsqlConnection connection, [Optional] NpgsqlTransaction transaction)
    {
        string columns = String.Join(",", data.data.Select(x => x.Key));
        string values = String.Join(",", data.data.Select(x => $"@{x.Key}"));
        Dictionary<string, object> parameters = data.data;


        string query = $"INSERT INTO {data.table} ({columns}) VALUES ({values}) RETURNING *;";

        var result = transaction.IsNotNullOrEmpty() ? await connection.QueryAsync(query, parameters, transaction) : await connection.QueryAsync(query, parameters);
        return checkResult(result);
    }

    public async Task<Response> setAsync(UpdateContext data, NpgsqlConnection connection, [Optional] NpgsqlTransaction transaction)
    {
        string values = String.Join(",", data.data.Select(x => $"{x.Key} = @{x.Key}"));
        Dictionary<string, object> updatedata = data.data;
        Dictionary<string, object> wherevalues = data.whereParams;

        var parameters = updatedata.Concat(wherevalues);

        string query = $"UPDATE {data.table} SET {values} WHERE {data.where} RETURNING *;";

        var result = transaction.IsNotNullOrEmpty() ? await connection.QueryAsync(query, parameters, transaction) : await connection.QueryAsync(query, parameters);
        return checkResult(result);
    }

    public async Task<Response> deleteAsync(DeleteContext data, NpgsqlConnection connection, [Optional] NpgsqlTransaction transaction)
    {
        Dictionary<string, object> parameters = data.whereParams;

        string query = $"DELETE FROM {data.table}  WHERE {data.where} RETURNING *;";

        var result = transaction.IsNotNullOrEmpty() ? await connection.QueryAsync(query, parameters, transaction) : await connection.QueryAsync(query, parameters);

        return checkResult(result);
    }

    public async Task<Response> runAsync(RunContext context, NpgsqlConnection connection, [Optional] NpgsqlTransaction transaction)
    {


        var result = transaction.IsNotNullOrEmpty() ? await connection.QueryAsync(context.procedure, context.values, transaction) : await  connection.QueryAsync(context.procedure, context.values);
        return checkResult(result);
        
    }

    private Response checkResult(IEnumerable<dynamic>? result) {
        if(result == null) {
            throw new Exception("Execution fail");
        }
        else if(result.Count() == 0){
            throw new Exception("No data is executing");
        }
        else{
            return new Response
            {
                code = StatusCodes.Status200OK,
                rows = result.Select(x => x as IDictionary<string, object>).ToList()
            };
        }
    }

}