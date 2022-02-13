namespace DataService;
public class CreateRequest : IRequest {

   public string? table {get;set;}
   public List<Parameter>? data{get;set;} 

}