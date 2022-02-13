namespace DataService;
public class ListResponse : IResponse{

    public int page {get;set;}
    public int pageSize{get;set;}

    public long total{get;set;}
    
    public List<IDictionary<string,object>>? rows {get;set;}
    public ResultCode code { get;set; }
    public string? message { get;set; }
}