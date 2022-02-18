namespace DataService;
public class QuerySelect
{
    private string fields { get; set; }
    private Query query { get; set; }
    private string? orderBy { get; set; }
    private string? groupBy { get; set; }
    
    private int limit { get; set; } = 10;

    private int page { get;set;} = 1;

    public QuerySelect(Query _query, string _fields = "*")
    {
        fields = _fields;
        query = _query;
    }

    public QuerySelect Order(string order)
    {
        orderBy = order;
        return this;
    }

    public QuerySelect Group(string group)
    {
        groupBy = group;
        return this;
    }

    public QuerySelect Page(int page){
        page = page;
        return this;
    }

    public QuerySelect Limit(int limit){
        limit = limit;
        return this;
    }

    public GetRequest Request(){
        return new GetRequest {
            page = this.page,
            pageSize = this.limit,
            tables = this.query.tables,
            fields = this.fields,
            groupBy = this.groupBy,
            orderBy = this.orderBy,
            filter = new Filter {
                where = this.query.where,
                parameters = this.query.parameters
            }

        };
    }

}
