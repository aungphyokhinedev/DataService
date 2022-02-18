
using AplusExtension;
namespace DataService;
public class QueryDelete
{

    private Query query { get; set; }

    public QueryDelete(Query _query)
    {
        query = _query;
    }

    public RemoveRequest Request()
    {
        if (this.query.IsNotNullOrEmpty() && this.query.where.IsNotNullOrEmpty())
        {
            return new RemoveRequest
            {
                table = this.query.tables,
                filter = new Filter
                {
                    where = this.query.where,
                    parameters = this.query.parameters
                },
            };
        }
        else
        {
            throw new Exception("Where clause is required for delete query");
        }


    }

}