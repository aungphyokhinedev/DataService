
using Npgsql;
using DataService;
using AplusExtension;

public class PostgresDataContext : IDataContext
{
    IConfiguration _config;

    public PostgresDataContext(IConfiguration cofig)
    {
        _config = cofig;
    }

    public async Task<ListResponse> GetListAsync(SelectContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.getAsync(data, connection);

            }
            catch (Exception e)
            {
                return new ListResponse
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
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
                return await PostgresFunction.insertAsync(data, connection);

            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
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
                return await PostgresFunction.setAsync(data, connection);


            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
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
                return await PostgresFunction.deleteAsync(data, connection);

            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.InternalServerError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }
    }

    ///this is for procedure call transaction control already set, not need to do in store procedure
    public async Task<Response> RunProcedureAsync(RunContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await PostgresFunction.runAsync(data, connection);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new Response
                        {
                            code = ResultCode.DatabaseError,
                            message = ex.Message
                        };
                    }

                }

            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }
    }


    //in this methods all queries will execute under transaction control
    //to fetch data from one query result use extra value params
    public async Task<IResponse> TransactionAsync(List<QueryContext> requests)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                Dictionary<string,IResponse> responses = new Dictionary<string,IResponse>();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int i = 0;

                        foreach (var request in requests)
                        {
                            //extra values -> to fetch data from previous query results
                            Dictionary<string,object> extra = getExtraValues(request,responses);

                            //tag name will be given in request initailization or given by auto serial
                            //0,1,2,.. and so on, can call tag by serial no as well 
                            string tag = request.tag.IsNotNullOrEmpty() ? request.tag : i.ToString();
                            if (request.GetType() == typeof(SelectContext))
                            {
                                var result = await PostgresFunction.getAsync((SelectContext)request, connection, transaction);
                                responses.Add(tag,result);
                            }
                            
                            if (request.GetType() == typeof(InsertContext))
                            {         
                                var insertrequest = (InsertContext)request;
                                insertrequest.data.AddDictionary(extra);
                                var result = await PostgresFunction.insertAsync(insertrequest, connection, transaction);
                                responses.Add(tag,result);
                            }
                            if (request.GetType() == typeof(UpdateContext))
                            {
                                var updaterequest = (UpdateContext)request;
                                updaterequest.whereParams.AddDictionary(extra);
                                var result = await PostgresFunction.setAsync(updaterequest, connection, transaction);
                                responses.Add(tag,result);
                            }
                            if (request.GetType() == typeof(DeleteContext))
                            {
                                var deleterequest = (DeleteContext)request;
                                deleterequest.whereParams.AddDictionary(extra);
                                var result = await PostgresFunction.deleteAsync(deleterequest, connection, transaction);
                                responses.Add(tag,result);
                            }
                            i++;
                        }

                        transaction.Commit();


                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new Response
                        {
                            code = ResultCode.DatabaseError,
                            message = ex.Message
                        };
                    }
                }

                return responses.Last().Value;
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }
    }

    //getting extra value from previous query results by tag name and file name
    private Dictionary<string,object> getExtraValues(QueryContext context, Dictionary<string,IResponse> responses){
        
        var newvalues = new Dictionary<string,object>();
        if(context.extraValues.IsNotNullOrEmpty()){
            
            context.extraValues.ForEach(x=>{
              var response =  responses.Where(response=>response.Key == x.tag).FirstOrDefault().Value;
              
              var data = new Dictionary<string, object>(response.rows.FirstOrDefault());

              object newvalue = data.Where(d=>d.Key == x.fieldName ).FirstOrDefault().Value;
              newvalues.Add(x.parameterName,newvalue);
            });
        }
        return newvalues;
    }


}