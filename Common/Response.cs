namespace DataService;
public class Response : IResponse{
    public List<IDictionary<string,object>>? rows{get;set;} 
}