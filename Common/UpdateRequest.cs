namespace DataService;
public class UpdateRequest {

   public string? table {get;set;}
   public List<Parameter>? data{get;set;} 
   public Filter? filter{get;set;}

}