using AplusExtension;
namespace DataService;
public class QueryUpdate
{
    private List<Parameter> data { get; set; }
    private Query query { get; set; }

    public QueryUpdate(Query _query, Dictionary<string, object> _data)
    {
        data = _data.toParameterList();
        query = _query;
    }

    public UpdateRequest Request(){
      if(this.query.IsNotNullOrEmpty() && this.query.where.IsNotNullOrEmpty()) {
        return new UpdateRequest {
            table = this.query.tables,
            filter = new Filter {
                where = this.query.where,
                parameters = this.query.parameters
            },
            data = this.data
        };
      }
      else {
           throw new Exception("Where clause is required for update query");
      }
    }
}