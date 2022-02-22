public interface IResponse {
 
   int code{get;set;}
   string? message{get;set;}
   
   List<IDictionary<string,object>>? rows{get;set;} 

}