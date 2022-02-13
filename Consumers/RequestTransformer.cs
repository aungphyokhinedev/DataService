using AplusExtension;
using DataService;
using Newtonsoft.Json;

public class RequestTransformer {
     public static SelectContext toSelect(GetRequest request){
        return new SelectContext {
            page =  request.page,
            pageSize = request.pageSize,
            fields = request.fields,
            where = request.filter.IsNotNullOrEmpty() ? request.filter.where : null,
            whereParams = request.filter.IsNotNullOrEmpty() ?request.filter.parameters.toDictionaryList() : null,
            tables = request.tables,
            orderBy = request.orderBy,
            groupBy = request.groupBy
        };
    }

    public static InsertContext toInsert(CreateRequest request){

        return new InsertContext {
            table = request.table,
            data = request.data.IsNotNullOrEmpty() ? request.data.toDictionaryList() : null
        };
    }

    public static UpdateContext toUpdate(UpdateRequest request){
        return new UpdateContext {
            table = request.table,
            data = request.data.IsNotNullOrEmpty() ? request.data.toDictionaryList() : null,
            where = request.filter.IsNotNullOrEmpty() ? request.filter.where : null,
            whereParams = request.filter.IsNotNullOrEmpty() ?request.filter.parameters.toDictionaryList() : null,
            
        };
    }

    public static DeleteContext toDelete(RemoveRequest request){
        return new DeleteContext {
            table = request.table,
            where = request.filter.IsNotNullOrEmpty() ? request.filter.where : null,
            whereParams = request.filter.IsNotNullOrEmpty() ?request.filter.parameters.toDictionaryList() : null,
            
        };
    }

    public static List<QueryContext> toListRequests(string json){
       var data = JsonConvert.DeserializeObject<List<QueryRequest>>(json);
       List<QueryContext> requests = new List<QueryContext>();
       data.ForEach(x=>{
           if(x.type == typeof(SelectContext)){
               requests.Add(JsonConvert.DeserializeObject<SelectContext>(x.request));
           }
           if(x.type == typeof(InsertContext)){
               requests.Add(JsonConvert.DeserializeObject<InsertContext>(x.request));
           }
           if(x.type == typeof(UpdateContext)){
               requests.Add(JsonConvert.DeserializeObject<UpdateContext>(x.request));
           }
           if(x.type == typeof(DeleteContext)){
               requests.Add(JsonConvert.DeserializeObject<DeleteContext>(x.request));
           }
       });

       return requests;
    }
}