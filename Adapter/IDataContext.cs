using DataService;


public interface IDataContext  {

    Task<ListResponse>  GetListAsync(GetRequest request);

    Task<Response> AddAsync(CreateRequest request);

    Task<Response> UpdateAsync(UpdateRequest request);

    Task<Response> RemoveAsync(RemoveRequest request);
}