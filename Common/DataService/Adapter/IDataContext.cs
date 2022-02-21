namespace DataService;


public interface IDataContext  {

    Task<ListResponse>  GetListAsync(SelectContext data);

    Task<Response> AddAsync(InsertContext data);

    Task<Response> UpdateAsync(UpdateContext request);

    Task<Response> RemoveAsync(DeleteContext request);

    Task<Response> TransactionAsync(List<QueryContext> requests);
}