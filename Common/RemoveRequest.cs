namespace DataService;
public class RemoveRequest  : IRequest{

   public string? table {get;set;}
   public Filter? filter{get;set;}

}