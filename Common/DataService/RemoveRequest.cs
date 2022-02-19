namespace DataService;
public class RemoveRequest  : QueryRequest{

   public string? table {get;set;}
   public Filter? filter{get;set;}

   
}