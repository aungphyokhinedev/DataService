namespace AplusDbContext;
public class GetRequest
{
   public int page {get;set;}
   public int pageSize{get;set;}
   public string fields{get;set;}
   public Filter filter{get;set;}
   public string? tables {get;set;}

   public string? orderBy {get;set;}
   public string? groupBy {get;set;}
}