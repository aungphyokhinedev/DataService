namespace DataService;
public class UpdateRequest  : IRequest{

   public string? table {get;set;}
   public List<Parameter>? data{get;set;} 
   public Filter? filter{get;set;}

}