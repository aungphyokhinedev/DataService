using AplusExtension;
namespace DataService;

public class QueryInsert
{
    public List<Parameter> data { get; set; }
    private Query query { get; set; }


    public QueryInsert(Query _query, Dictionary<string, object> _data)
    {
        data = _data.toParameterList();
        query = _query;
    }

    public CreateRequest Request(){
        return new CreateRequest {
            table = this.query.tables,
            data = this.data
        };
    }


}