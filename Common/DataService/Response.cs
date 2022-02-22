namespace DataService;
public class Response : IResponse{
    public int code { get;set; }
    public string? message { get;set; }
    public List<IDictionary<string,object>>? rows{get;set;} 
}